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
using Gooios.OrderService.Repositories;
using Microsoft.EntityFrameworkCore;
using Gooios.ActivityService.Filters;
using Gooios.OrderService.Configurations;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Gooios.GooiosService.Interceptors;
using AspectCore.Configuration;
using Gooios.OrderService.Domains.Events;
using Gooios.OrderService.Applications.Services;
using Gooios.OrderService.Domains.Repositories;
using Gooios.OrderService.Proxies;

namespace Gooios.OrderService
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

            services.AddTransient<IOrderAppService, OrderAppService>();
            services.AddTransient<IDeliveryNoteAppService, DeliveryNoteAppService>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IDeliveryNoteRepository, DeliveryNoteRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IOrderTraceRepository, OrderTraceRepository>();

            services.AddTransient<IGoodsServiceProxy, GoodsServiceProxy>();
            services.AddTransient<IFancyServiceProxy, FancyServiceProxy>();
            services.AddTransient<IAuthServiceProxy, AuthServiceProxy>();
            services.AddTransient<IActivityServiceProxy, ActivityServiceProxy>();

            services.AddTransient<IDomainEventHandler<OrderCancelledEvent>, OrderCancelledEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderCompletedEvent>, OrderCompletedEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderPaidEvent>, OrderPaidEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderPaidFailedEvent>, OrderPaidFailedEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderRefundedEvent>, OrderRefundedEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderShippedEvent>, OrderShippedEventHandler>();
            services.AddTransient<IDomainEventHandler<OrderSubmittedEvent>, OrderSubmittedEventHandler>();

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
