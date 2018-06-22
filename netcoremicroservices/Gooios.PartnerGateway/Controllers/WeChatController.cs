using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.PartnerGatewayService.Applications.DTO;
using Gooios.PartnerGateway.Applications.Services;
using Gooios.Infrastructure.Logs;
using Newtonsoft.Json;

namespace Gooios.PartnerGateway.Controllers
{
    [Produces("application/json")]
    [Route("api/wechat/v1")]
    public class WeChatController : Controller
    {
        readonly IWeChatAppService _wechatAppService;
        public WeChatController(IWeChatAppService wechatAppService)
        {
            _wechatAppService = wechatAppService;
        }

        [HttpPost]
        [Route("paymentnotify")]
        public async Task PaymentNotify([FromBody]WeChatPaymentNotifyMessageDTO model)
        {
            var logTask = LogProvider.Trace(new Log
            {
                ApplicationKey = "968960bff18111e799160126c7e9f008",
                AppServiceName = "PartnerGateway",
                BizData = JsonConvert.SerializeObject(model),
                CallerApplicationKey = "wechat",
                Exception = "",
                Level = LogLevel.INFO,
                LogTime = DateTime.Now,
                Operation = "paymentnotify",
                ReturnValue = ""
            });

            await _wechatAppService.WeChatPaymentNotify(model);
        }
    }
}