using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.PaymentService.Applications.DTO;
using PaySharp.Core;
using Gooios.PaymentService.Applications.Services;
using Gooios.Infrastructure.Logs;
using Newtonsoft.Json;
using Gooios.PaymentService.Proxies.DTOs;

namespace Gooios.PaymentService.Controllers
{
    [Produces("application/json")]
    [Route("api/wechatpayment/v1")]
    public class WeChatPaymentController : BaseApiController
    {
        readonly IWeChatPaymentAppService _wechatPaymentAppService;
        readonly IWeChatPaymentNotifyMessageAppService _notifyMessageAppService;

        public WeChatPaymentController(IWeChatPaymentAppService wechatPaymentAppService, IWeChatPaymentNotifyMessageAppService notifyMessageAppService)
        {
            _wechatPaymentAppService = wechatPaymentAppService;
            _notifyMessageAppService = notifyMessageAppService;
        }

        [HttpGet]
        [Route("getopenid/{code}/{organizationId}")]
        public async Task<string> GetOpenId(string code, string organizationId = null)
        {
            return await _wechatPaymentAppService.GetOpenId(code, organizationId);
        }

        [HttpPost]
        [Route("session")]
        public async Task<OpenIDSessionKeyDTO> InitSession([FromBody]InitSessionModel model)
        {
            return await _wechatPaymentAppService.GetSessionKey(model.Code,model.OrganizationId, "wx0a5983b08057acd0",model.NickName,model.PortraitUrl);
        }

        [HttpPost]
        [Route("repuestpayment")]
        public async Task<RequestPaymentResponseDTO> RequestPayment([FromBody]RequestPaymentRequestDTO model)
        {
            return await _wechatPaymentAppService.RequestPayment(model);
        }

        [HttpPost]
        [Route("paymentnotify")]
        public void Post([FromBody]WeChatPaymentNotifyMessageDTO model)
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
            _notifyMessageAppService.AddWeChatPaymentNotifyMessage(model);
        }

        [HttpPost]
        [Route("syncorderstate")]
        public async Task SyncWeChatOrderState(string orderId)
        {
            await _wechatPaymentAppService.SyncOrderStatus(orderId);
        }

        [HttpPut]
        [Route("setpaidsuccessed/{orderId}")]
        public async Task SetPaidSuccessed(string orderId)
        {
            await _wechatPaymentAppService.SetPaidSuccessed(orderId);
        }

        [HttpPut]
        [Route("setpaidfailed/{orderId}")]
        public async Task SetPaidFailed(string orderId)
        {
            await _wechatPaymentAppService.SetPaidFailed(orderId);
        }
    }

    public class InitSessionModel
    {
        public string Code { get; set; }

        public string NickName { get; set; } = "";

        public string PortraitUrl { get; set; } = "";

        public string OrganizationId { get; set; } = "";
    }
}