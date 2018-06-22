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
using Gooios.GoodsService.Repositories;
using Microsoft.EntityFrameworkCore;
using Gooios.GoodsService.Filters;
using Gooios.GoodsService.Configurations;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure;
using Gooios.GoodsService.Applications.Services;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.GoodsService.Domains.Services;
using Gooios.GooiosService.Interceptors;
using Gooios.GoodsService.Proxies;
using Gooios.GoodsService.Domains.Events;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Gooios.GoodsService
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

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ServiceDb"));
            });

            /* 替换
            services.AddSingleton<IEventBus, EventBus>();
            services.AddTransient<IEventAggregator, EventAggregator>();

            services.AddSingleton<IServiceConfigurationProxy, ServiceConfigurationProxy>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ServiceDb"));
            }, ServiceLifetime.Scoped);

            services.AddTransient<IGoodsService, Domains.Services.GoodsService>();

            services.AddTransient<IGoodsAppService, GoodsAppService>();
            services.AddTransient<IGoodsCategoryAppService, GoodsCategoryAppService>();

            services.AddTransient<IGoodsRepository, GoodsRepository>();
            services.AddTransient<IGoodsCategoryRepository, GoodsCategoryRepository>();
            services.AddTransient<IGoodsImageRepository, GoodsImageRepository>();
            services.AddTransient<IGrouponConditionRepository, GrouponConditionRepository>();

            services.AddTransient<IOnlineGoodsRepository, OnlineGoodsRepository>();
            services.AddTransient<IOnlineGoodsImageRepository, OnlineGoodsImageRepository>();
            services.AddTransient<IOnlineGrouponConditionRepository, OnlineGrouponConditionRepository>();

            services.AddTransient<IImageServiceProxy, ImageServiceProxy>();

            services.AddTransient<IDomainEventHandler<GoodsShelvedEvent>, GoodsShelvedEventHandler>();
            services.AddTransient<IDomainEventHandler<GoodsSoldOutEvent>, GoodsSoldOutEventHandler>();

            //services.AddTransient<IDomainEventHandler<VerificationCreatedEvent>, VerificationCreatedEventHandler>(); 

            var container = services.ToServiceContainer();

            container.Configure(config =>
            {
                config.Interceptors.AddTyped<ExceptionInterceptor>(m => m.DeclaringType.Name.EndsWith("AppService"));
            });
            

            return IocProvider.Container = container.Build();
            */

            var builder = new ContainerBuilder();
            
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().InstancePerDependency();
            builder.RegisterType<ServiceConfigurationProxy>().As<IServiceConfigurationProxy>().SingleInstance();
            builder.RegisterType<DbUnitOfWork>().As<IDbUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();
            //builder.RegisterType<DatabaseContext>().InstancePerLifetimeScope();

            builder.RegisterType<Domains.Services.GoodsService>().As<Domains.Services.IGoodsService>().InstancePerDependency();

            builder.RegisterType<GoodsAppService>().As<IGoodsAppService>().InstancePerDependency();
            builder.RegisterType<GoodsCategoryAppService>().As<IGoodsCategoryAppService>().InstancePerDependency();
            builder.RegisterType<GoodsCategoryRepository>().As<IGoodsCategoryRepository>().InstancePerDependency();
            builder.RegisterType<GoodsRepository>().As<IGoodsRepository>().InstancePerDependency();
            builder.RegisterType<GoodsImageRepository>().As<IGoodsImageRepository>().InstancePerDependency();
            builder.RegisterType<GrouponConditionRepository>().As<IGrouponConditionRepository>().InstancePerDependency();
            builder.RegisterType<OnlineGoodsRepository>().As<IOnlineGoodsRepository>().InstancePerDependency();
            builder.RegisterType<OnlineGoodsImageRepository>().As<IOnlineGoodsImageRepository>().InstancePerDependency();
            builder.RegisterType<OnlineGrouponConditionRepository>().As<IOnlineGrouponConditionRepository>().InstancePerDependency();

            builder.RegisterType<ImageServiceProxy>().As<IImageServiceProxy>().InstancePerDependency();
            builder.RegisterType<AmapProxy>().As<IAmapProxy>().InstancePerDependency();
            builder.RegisterType<AuthServiceProxy>().As<IAuthServiceProxy>().InstancePerDependency();
            builder.RegisterType<OrderServiceProxy>().As<IOrderServiceProxy>().InstancePerDependency();
            builder.RegisterType<OrganizationServiceProxy>().As<IOrganizationServiceProxy>().InstancePerDependency();
            builder.RegisterType<ActivityServiceProxy>().As<IActivityServiceProxy>().InstancePerDependency();

            builder.RegisterType<GoodsShelvedEventHandler>().As<IDomainEventHandler<GoodsShelvedEvent>>().InstancePerDependency();
            builder.RegisterType<GoodsSoldOutEventHandler>().As<IDomainEventHandler<GoodsSoldOutEvent>>().InstancePerDependency();
            builder.RegisterType<GoodsAppService>().As<IGoodsAppService>().InstancePerDependency();
            
            builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerDependency();
            builder.RegisterType<CommentImageRepository>().As<ICommentImageRepository>().InstancePerDependency();
            builder.RegisterType<CommentTagRepository>().As<ICommentTagRepository>().InstancePerDependency();
            builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerDependency();

            builder.RegisterType<CommentAppService>().As<ICommentAppService>().InstancePerDependency();
            builder.RegisterType<GoodsTagStatisticsAppService>().As<IGoodsTagStatisticsAppService>().InstancePerDependency();
            builder.RegisterType<TagAppService>().As<ITagAppService>().InstancePerDependency();

            builder.RegisterType<TmpInstanceGenerate>().InstancePerDependency();

            builder.Populate(services);
            IocProvider.Container = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(IocProvider.Container);
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
