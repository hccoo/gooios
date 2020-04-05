using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gooios.AuthorizationService.Data;
using Gooios.AuthorizationService.Models;
using Gooios.AuthorizationService.Services;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Gooios.AuthorizationService.Configurations;
using IdentityServer4.EntityFramework.Mappers;
using Gooios.AuthorizationService.Proxies;
using Gooios.AuthorizationService.Core;

namespace Gooios.AuthorizationService
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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddSingleton<IServiceConfigurationProxy, ServiceConfigurationProxy>();
            services.AddTransient<IVerificationProxy, VerificationProxy>();
            services.AddTransient<IAppletUserService, AppletUserService>();
            services.AddTransient<IPaymentServiceProxy, PaymentServiceProxy>();

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddOptions();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // 配置身份选项
                // 密码配置
                options.Password.RequireDigit = false;//是否需要数字(0-9).
                options.Password.RequiredLength = 6;//设置密码长度最小为6
                options.Password.RequireNonAlphanumeric = false;//是否包含非字母或数字字符。
                options.Password.RequireUppercase = false;//是否需要大写字母(A-Z).
                options.Password.RequireLowercase = false;//是否需要小写字母(a-z).

                // 锁定设置
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);//账户锁定时长30分钟
                options.Lockout.MaxFailedAccessAttempts = 10;//10次失败的尝试将账户锁定

                // 用户设置
                options.User.RequireUniqueEmail = false; //是否Email地址必须唯一
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(@"./certificate/gooios.pfx", "!QAZ2wsx098", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable))
                .AddTestUsers(InMemoryConfiguration.Users().ToList())
                .AddConfigurationStore<ConfigurationCustomDbContext>(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore<PersistedGrantCustomDbContext>(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600 * 24 * 7;
                })
                .AddAspNetIdentity<ApplicationUser>()
                //.AddResourceOwnerValidator<SessionKeyValidator>()
                .AddResourceOwnerValidator<CookAppSessionKeyValidator>()
                .AddProfileService<ProfileService>();



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantCustomDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationCustomDbContext>();
                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();
                appDbContext.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in InMemoryConfiguration.Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in InMemoryConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in InMemoryConfiguration.ApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
