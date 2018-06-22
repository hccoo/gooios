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
using Gooios.Infrastructure;
using Gooios.ImageService.Repositories;
using Gooios.ImageService.Configurations;
using Gooios.Infrastructure.Events;
using Gooios.ImageService.Filters;
using Gooios.ImagesService.Filters;
using Microsoft.EntityFrameworkCore;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Gooios.ImageService.Interceptors;
using AspectCore.Configuration;
using Gooios.ImageService.Applications.Services;
using Gooios.ImageService.Domains.Repositories;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace Gooios.ImageService
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

            services.AddTransient<IImageAppService, ImageAppService>();
            services.AddTransient<IImageRepository, ImageRepository>();
            //services.AddCors();
            //services.AddTransient<IDomainEventHandler<VerificationCreatedEvent>, VerificationCreatedEventHandler>();

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
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"uploadimages")),
                RequestPath = new PathString("/uploadimages")
            });
            app.UseMvc();
            //app.UseCors(builder =>
            //{
            //    builder.AllowAnyHeader();
            //    builder.AllowAnyMethod();
            //    //builder.WithOrigins("https://enterpriseportal.gooios.com","https://www.arcanestars.com","https://localhost:44347");
            //    builder.AllowAnyOrigin();
            //});
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
