using Microsoft.AspNetCore.Mvc;
using Gooios.PaymentService.Applications.Services;
using Gooios.PaymentService.Applications.DTO;
using NLog;
using Newtonsoft.Json;

namespace Gooios.PaymentService.Controllers
{
    [Produces("application/json")]
    [Route("api/wechatpaymentnotify/v1")]
    public class WeChatPaymentNotifyMessageController : BaseApiController
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        readonly IWeChatPaymentNotifyMessageAppService _notifyMessageAppService;
        public WeChatPaymentNotifyMessageController(IWeChatPaymentNotifyMessageAppService notifyMessageAppService)
        {
            _notifyMessageAppService = notifyMessageAppService;
        }

        [HttpPost]
        public void Post([FromBody]WeChatPaymentNotifyMessageDTO model)
        {
            var modelJson = JsonConvert.SerializeObject(model);
            _logger.Info($"wechatpaymentnotify - model:{modelJson}");
            _notifyMessageAppService.AddWeChatPaymentNotifyMessage(model);
        }
    }
}