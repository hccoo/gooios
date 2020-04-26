using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Gooios.UserService.Configurations;
using Gooios.UserService.Core;
using Gooios.UserService.Interceptors;
using Gooios.UserService.Proxies;
using Gooios.UserService.Repositories;
using Gooios.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Gooios.AuthService.Proxies;
using Gooios.AuthService.Data;
using Gooios.AuthService.Core;

namespace Gooios.UserService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("ServiceDb");
            var userDbConnString = Configuration.GetConnectionString("UserDb");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddOptions();

            services.AddSingleton<IServiceConfigurationAgent, ServiceConfiguration>();
            services.AddTransient<IVerificationProxy, VerificationProxy>();
            services.AddTransient<IWechatProxy, WechatProxy>();
            services.AddTransient<IPaymentServiceProxy, PaymentServiceProxy>();
            services.AddTransient<IUserServiceProxy, UserServiceProxy>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<PartnerAuthCodeValidator>();
            services.AddTransient<VerifyCodeValidator>();
            services.AddTransient<WechatAppletValidator>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<ConfigurationCustomDbContext>(options => options.UseMySql(connectionString));
            services.AddDbContext<PersistedGrantCustomDbContext>(options => options.UseMySql(connectionString));

            services.AddIdentityServer()
                    .AddSigningCredential(new X509Certificate2(@"./certificate/gooios.pfx", "!QAZ2wsx098", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable))
                    .AddTestUsers(DataConfiguration.GetUsers().ToList())
                    .AddConfigurationStore<ConfigurationCustomDbContext>(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddOperationalStore<PersistedGrantCustomDbContext>(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 7200;
                    })
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddExtensionGrantValidator<VerifyCodeValidator>()
                    .AddExtensionGrantValidator<PartnerAuthCodeValidator>()
                    .AddExtensionGrantValidator<WechatAppletValidator>()
                    .AddProfileService<ProfileService>();

            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<ExceptionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
                //config.Interceptors.AddTyped<TransactionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
                config.Interceptors.AddTyped<ProxyInterceptor>(m => m.DeclaringType.Name.EndsWith("Proxy"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthentication();
            app.UseIdentityServer();
            //app.UseAuthorization();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

                Console.WriteLine("开始初始化 PersistedGrantDbContext ...");
                #region PersistedGrantDbContext

                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantCustomDbContext>().Database.Migrate();

                #endregion

                Console.WriteLine("开始初始化 ConfigurationDbContext ...");
                #region ConfigurationDbContext 

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationCustomDbContext>();
                context.Database.Migrate();

                Console.WriteLine("开始初始化 Clients ...");
                if (!context.Clients.Any())
                {
                    foreach (var client in DataConfiguration.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                Console.WriteLine("开始初始化 IdentityResources ...");
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in DataConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                Console.WriteLine("开始初始化 ApiResources ...");
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in DataConfiguration.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                #endregion


            }
        }
    }
}
