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
using Gooios.FancyService.Interceptors;
using AspectCore.Injector;
using AspectCore.Configuration;
using Gooios.FancyService.Domains.Repositories;
using Gooios.FancyService.Repositories;
using Gooios.FancyService.Configurations;
using Gooios.FancyService.Domains.Events;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure;
using Gooios.FancyService.Filters;
using Microsoft.EntityFrameworkCore;
using Gooios.FancyService.Applications.Services;
using Gooios.FancyService.Proxies;

namespace Gooios.FancyService
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


            services.AddTransient<ICommentImageRepository, CommentImageRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentTagRepository, CommentTagRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IServiceImageRepository, ServiceImageRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IServicerImageRepository, ServicerImageRepository>();
            services.AddTransient<IServicerRepository, ServicerRepository>();
            services.AddTransient<ITagRepository, TagRepository>();

            services.AddTransient<ICategoryAppService, CategoryAppService>();
            services.AddTransient<ICommentAppService, CommentAppService>();
            services.AddTransient<IReservationAppService, ReservationAppService>();
            services.AddTransient<IServiceAppService, ServiceAppService>();
            services.AddTransient<IServicerAppService, ServicerAppService>();

            services.AddTransient<IImageServiceProxy, ImageServiceProxy>();
            services.AddTransient<IOrderServiceProxy, OrderServiceProxy>();
            services.AddTransient<IOrganizationServiceProxy, OrganizationServiceProxy>();
            services.AddTransient<IAuthServiceProxy, AuthServiceProxy>();
            services.AddTransient<IAmapProxy, AmapProxy>();
            services.AddTransient<ITagStatisticsAppService, TagStatisticsAppService>();
            services.AddTransient<ITagAppService, TagAppService>();

            services.AddTransient<TmpInstanceGenerate>();

            services.AddTransient<IDomainEventHandler<AppointmentConfirmedEvent>, AppointmentConfirmedEventHandler>();
            
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
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
            }
        }
    }
}
