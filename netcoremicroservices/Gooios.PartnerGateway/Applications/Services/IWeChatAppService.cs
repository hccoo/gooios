using Gooios.Infrastructure;
using Gooios.PartnerGateway.Proxies;
using Gooios.PartnerGatewayService.Applications.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PartnerGateway.Applications.Services
{
    public interface IWeChatAppService : IApplicationServiceContract
    {
        Task WeChatPaymentNotify(WeChatPaymentNotifyMessageDTO model);
    }
    public class WeChatAppService : ApplicationServiceContract, IWeChatAppService
    {
        readonly IPaymentServiceProxy _paymentServiceProxy;

        public WeChatAppService(IPaymentServiceProxy paymentServiceProxy)
        {
            _paymentServiceProxy = paymentServiceProxy;
        }

        public async Task WeChatPaymentNotify(WeChatPaymentNotifyMessageDTO model)
        {
            await _paymentServiceProxy.WeChatPaymentNotify(model);
        }
    }
}
