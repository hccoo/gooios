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
using Gooios.PaymentGateway.Repositories;
using Gooios.PaymentGateway.Configurations;
using Gooios.Infrastructure.Events;
using Gooios.PaymentGateway.Filters;
using Gooios.ImagesService.Filters;
using Microsoft.EntityFrameworkCore;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Gooios.PaymentGateway.Interceptors;
using AspectCore.Configuration;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using ICanPay.Alipay;
using ICanPay.Core;
using ICanPay.Wechatpay;

namespace Gooios.PaymentGateway
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

            services.AddICanPay(a =>
            {
                //var gateways = new List<GatewayBase>();
                var gateways = new Gateways();
                // 设置商户数据
                var alipayMerchant = new ICanPay.Alipay.Merchant
                {
                    AppId = "2017093009005992",
                    NotifyUrl = "http://localhost:61337/Notify",
                    ReturnUrl = "http://localhost:61337/Return",
                    AlipayPublicKey = "Varorbc",
                    Privatekey = "Varorbc"
                };

                var wechatpayMerchant = new ICanPay.Wechatpay.Merchant
                {
                    AppId = "wx2428e34e0e7dc6ef",
                    MchId = "1233410002",
                    Key = "e10adc3849ba56abbe56e056f20f883e",
                    AppSecret = "51c56b886b5be869567dd389b3e5d1d6",
                    SslCertPath = "Certs/apiclient_cert.p12",
                    SslCertPassword = "1233410002",
                    NotifyUrl = "http://localhost:61337/Notify"
                };

                gateways.Add(new AlipayGateway(alipayMerchant));
                gateways.Add(new WechatpayGateway(wechatpayMerchant));

                return gateways;
            });

            services.AddSingleton<IEventBus, EventBus>();
            services.AddTransient<IEventAggregator, EventAggregator>();

            services.AddSingleton<IServiceConfigurationProxy, ServiceConfigurationProxy>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<DatabaseContext>(options => {
                options.UseMySql(Configuration.GetConnectionString("ServiceDb"));
            });

            //services.AddTransient<IImageAppService, ImageAppService>();
            //services.AddTransient<IImageRepository, ImageRepository>();

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
            app.UseICanPay();
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
