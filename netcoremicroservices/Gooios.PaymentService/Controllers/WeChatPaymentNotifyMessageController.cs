using Microsoft.AspNetCore.Mvc;
using Gooios.PaymentService.Applications.Services;
using Gooios.PaymentService.Applications.DTO;

namespace Gooios.PaymentService.Controllers
{
    [Produces("application/json")]
    [Route("api/wechatpaymentnotify/v1")]
    public class WeChatPaymentNotifyMessageController : BaseApiController
    {
        readonly IWeChatPaymentNotifyMessageAppService _notifyMessageAppService;
        public WeChatPaymentNotifyMessageController(IWeChatPaymentNotifyMessageAppService notifyMessageAppService)
        {
            _notifyMessageAppService = notifyMessageAppService;
        }

        [HttpPost]
        public void Post([FromBody]WeChatPaymentNotifyMessageDTO model)
        {
            _notifyMessageAppService.AddWeChatPaymentNotifyMessage(model);
        }
    }
}