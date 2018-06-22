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
using Gooios.ActivityService.Repositories;
using Microsoft.EntityFrameworkCore;
using Gooios.ActivityService.Filters;
using Gooios.ActivityService.Configurations;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure;
using Gooios.ActivityService.Domains.Events;
using AspectCore.Extensions.DependencyInjection;
using Gooios.ActivityService.Applications.Services;
using Gooios.ActivityService.Domains.Repositories;
using AspectCore.Injector;
using Gooios.ActivityService.Interceptors;
using AspectCore.Configuration;
using Gooios.ActivityService.Proxies;

namespace Gooios.ActivityService
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

            services.AddDbContext<DatabaseContext>(options => {
                options.UseMySql(Configuration.GetConnectionString("ServiceDb"));
            });

            services.AddTransient<IGrouponActivityAppService, GrouponActivityAppService>();
            services.AddTransient<IGrouponParticipationAppService, GrouponParticipationAppService>();
            services.AddTransient<ITopicAppService, TopicAppService>();

            services.AddTransient<IGrouponActivityRepository, GrouponActivityRepository>();
            services.AddTransient<IGrouponParticipationRepository, GrouponParticipationRepository>();
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddTransient<ITopicImageRepository, TopicImageRepository>();

            services.AddTransient<IAuthServiceProxy, AuthServiceProxy>();
            services.AddTransient<IImageServiceProxy, ImageServiceProxy>();
            services.AddTransient<IAmapProxy, AmapProxy>();
            services.AddTransient<IOrganizationServiceProxy, OrganizationServiceProxy>();

            services.AddTransient<IDomainEventHandler<GrouponParticipatedEvent>, GrouponParticipatedEventHandler>();

            var container = services.ToServiceContainer();

            container.Configure(config =>
            {
                config.Interceptors.AddTyped<ExceptionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
            });

            return IocProvider.Container = container.Build();
        }

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
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
            }
        }
    }
}
