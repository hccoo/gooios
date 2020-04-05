using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Gooios.Infrastructure;
using Gooios.UserService.Application.Services;
using Gooios.UserService.Configurations;
using Gooios.UserService.Domain.Repositories;
using Gooios.UserService.Interceptors;
using Gooios.UserService.Proxies;
using Gooios.UserService.Repositories;
using Gooios.UserServiceHost.Configurations;
using Gooios.UserServiceHost.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gooios.UserServiceHost
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
            services.AddMvc(options =>
            {
                options.Filters.Add<ApiKeyFilter>();
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<LogFilter>();
            });

            var connectionString = Configuration.GetConnectionString("ServiceDb");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddOptions();

            services.AddSingleton<IServiceConfigurationAgent, ServiceConfiguration>();
            services.AddTransient<IVerificationProxy, VerificationProxy>();
            services.AddTransient<IWechatProxy, WechatProxy>();
            services.AddTransient<IPaymentServiceProxy, PaymentServiceProxy>();

            services.AddTransient<ICookAppUserRepository, CookAppUserRepository>();
            services.AddTransient<ICookAppPartnerLoginUserRepository, CookAppPartnerLoginUserRepository>();
            services.AddTransient<IUserAppService, UserAppService>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<ServiceDbContext>(options => options.UseMySql(connectionString));

            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<ExceptionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
                config.Interceptors.AddTyped<TransactionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
                config.Interceptors.AddTyped<ProxyInterceptor>(m => m.DeclaringType.Name.EndsWith("Proxy"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeDatabase(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                #region PersistedGrantDbContext

                serviceScope.ServiceProvider.GetRequiredService<ServiceDbContext>().Database.Migrate();

                #endregion
            }
        }
    }
}
