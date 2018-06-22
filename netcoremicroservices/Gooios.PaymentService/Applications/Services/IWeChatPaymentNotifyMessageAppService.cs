using Gooios.Infrastructure;
using Gooios.PaymentService.Applications.DTO;
using Gooios.PaymentService.Domains.Aggregates;
using Gooios.PaymentService.Domains.Repositories;
using Gooios.PaymentService.Proxies;
using Newtonsoft.Json;
using PaySharp.Core;
using PaySharp.Wechatpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Applications.Services
{
    public interface IWeChatPaymentNotifyMessageAppService : IApplicationServiceContract
    {
        Task AddWeChatPaymentNotifyMessage(WeChatPaymentNotifyMessageDTO model);
    }

    public class WeChatPaymentNotifyMessageAppService : ApplicationServiceContract, IWeChatPaymentNotifyMessageAppService
    {
        readonly IGateways _gateways;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IWeChatPaymentNotifyMessageRepository _wechatPaymentNotifyMessageRepository;
        readonly IOrderServiceProxy _orderServiceProxy;
        readonly IWeChatAppConfigurationRepository _wechatAppConfigurationRepository;

        public WeChatPaymentNotifyMessageAppService(
            IDbUnitOfWork dbUnitOfWork, 
            IWeChatPaymentNotifyMessageRepository wechatPaymentNotifyMessageRepository, 
            IOrderServiceProxy orderServiceProxy,
            IWeChatAppConfigurationRepository wechatAppConfigurationRepository,
            IGateways gateways)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _wechatPaymentNotifyMessageRepository = wechatPaymentNotifyMessageRepository;
            _orderServiceProxy = orderServiceProxy;
            _wechatAppConfigurationRepository = wechatAppConfigurationRepository;
            _gateways = gateways;
        }

        public async Task AddWeChatPaymentNotifyMessage(WeChatPaymentNotifyMessageDTO model)
        {
            var orderDTO = await _orderServiceProxy.GetByOrderNo(model.OutTradeNo);

            if (orderDTO == null) return;
            var organizationId = "";
            IGateway gateway = null;
            organizationId = orderDTO.OrganizationId ?? "";
            WeChatAppConfiguration appConfig = null;
            if (!string.IsNullOrEmpty(organizationId))
            {
                appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == organizationId).FirstOrDefault();
            }

            if (appConfig != null)
            {
                var wechatpayMerchant = new Merchant
                {
                    AppId = appConfig.AppId,
                    MchId = appConfig.MchId,
                    Key = appConfig.Key,
                    AppSecret = appConfig.AppSecret,
                    SslCertPath = appConfig.SslCertPath,
                    SslCertPassword = appConfig.SslCertPassword,
                    NotifyUrl = appConfig.NotifyUrl
                };

                gateway = new WechatpayGateway(wechatpayMerchant);
            }
            else
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }

            var key = ((WechatpayGateway)(gateway)).Merchant.Key;
            //if (model.Sign.ToUpper() != model.ToSign(key).ToUpper()) return;

            _wechatPaymentNotifyMessageRepository.Add(new WeChatPaymentNotifyMessage
            {
                CreatedOn = DateTime.Now,
                NotifyApiName = "AddWeChatPaymentNotifyMessage",
                MessageContent = JsonConvert.SerializeObject(model)
            });
            _dbUnitOfWork.Commit();
        }
    }
}
