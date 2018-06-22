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
using PaySharp.Core;
using System.IO;
using PaySharp.Alipay;
using PaySharp.Wechatpay;
using Gooios.Infrastructure.Events;
using Gooios.PaymentService.Configurations;
using Gooios.Infrastructure;
using Gooios.PaymentService.Repositories;
using Microsoft.EntityFrameworkCore;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Gooios.PaymentService.Interceptors;
using AspectCore.Configuration;
using Gooios.PaymentService.Filters;
using Gooios.PaymentService.Domains.Repositories;
using Gooios.PaymentService.Applications.Services;
using Gooios.PaymentService.Proxies;
using PaySharp.Wechatpay;
using PaySharp.Core;

namespace Gooios.PaymentService
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

            services.AddPaySharp(a =>
            {
                var gateways = new Gateways();
                var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                {
                    AppId = "wx0a5983b08057acd0",
                    AppSecret = "7a6e9a59bc4ea25379945979d5423b1e",
                    SslCertPath = "Certs/apiclient_cert.p12",
                    SslCertPassword = "1504096781",
                    MchId = "1504096781",
                    Key = "sCafbvydmAoLaNjFDfd2nfVMG86bKBY8",
                    //NotifyUrl = "https://partnergateway.gooios.com/api/wechat/v1/paymentnotify"
                    NotifyUrl = "https://apigateway.gooios.com/paymentservice/wechatpayment/v1/paymentnotify"
                };
                a.Add(new WechatpayGateway(wechatpayMerchant));
                a.UseWechatpay(Configuration);
            });

            services.AddSingleton<IEventBus, EventBus>();
            services.AddTransient<IEventAggregator, EventAggregator>();

            services.AddSingleton<IServiceConfigurationProxy, ServiceConfigurationProxy>();
            services.AddSingleton<IWeChatAppConfigurationRepository, WeChatAppConfigurationRepository>();

            services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
            services.AddScoped<IDbContextProvider, DbContextProvider>();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ServiceDb"));
            });

            services.AddTransient<IWeChatPaymentNotifyMessageRepository, WeChatPaymentNotifyMessageRepository>();
            services.AddTransient<IWeChatPaymentNotifyMessageAppService, WeChatPaymentNotifyMessageAppService>();
            services.AddTransient<IWeChatPaymentAppService, WeChatPaymentAppService>();
            services.AddTransient<IOrderServiceProxy, OrderServiceProxy>();
            services.AddTransient<IWeChatApiProxy, WeChatApiProxy>();

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

            app.UseMvc();
            app.UsePaySharp();
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
