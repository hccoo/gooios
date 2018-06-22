using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspectCore.Extensions.DependencyInjection;
using Gooios.VerificationService.Application.Services;
using Gooios.VerificationService.Interceptors;
using AspectCore.Injector;
using AspectCore.Configuration;
using Gooios.VerificationService.Domain.Repositories;
using Gooios.VerificationService.Repositories;
using Gooios.VerificationService.Domain.Services;
using Gooios.VerificationService.Configurations;
using Gooios.VerificationService.Domain.Events;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure;
using Gooios.VerificationService.Filters;
using Microsoft.EntityFrameworkCore;
using Gooios.VerificationService.Proxies;

namespace Gooios.VerificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<ApiKeyFilter>();
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<LogFilter>();
            });
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddOptions();

            services.AddSingleton<IEventBus, EventBus>();
            services.AddTransient<IEventAggregator, EventAggregator>();

            services.AddSingleton<IServiceConfigurationProxy, ServiceConfigurationProxy>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<VerificationDbContext>(options=> {
                options.UseMySql(Configuration.GetConnectionString("VerificationServiceDb"));
            });

            services.AddTransient<IVerificationAppService, VerificationAppService>();
            services.AddTransient<IVerificationRepository, VerificationRepository>();
            services.AddTransient<IVerificationService, Domain.Services.VerificationService> ();
            services.AddTransient<ISmsProxy, FegineSmsProxy>();

            services.AddTransient<IDomainEventHandler<VerificationCreatedEvent>, VerificationCreatedEventHandler>();

            var container = services.ToServiceContainer();

            container.Configure(config =>
            {
                config.Interceptors.AddTyped<ExceptionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
            });

            return IocProvider.Container = container.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

            app.UseMvc();
        }

        void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<VerificationDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
