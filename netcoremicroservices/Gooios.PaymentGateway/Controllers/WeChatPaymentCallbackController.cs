using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ICanPay.Core;
using ICanPay.Wechatpay;

namespace Gooios.PaymentGateway.Controllers
{
    [Produces("application/json")]
    [Route("api/WeChatPaymentCallback")]
    public class WeChatPaymentCallbackController : Controller
    {
        private readonly IGateways _gateways;

        public WeChatPaymentCallbackController(IGateways gateways)
        {
            _gateways = gateways;

            Receive();
        }

        async Task Receive()
        {
            PaymentNotify notify = new PaymentNotify(_gateways);
            notify.PaymentSucceed += Notify_PaymentSucceed;
            notify.PaymentFailed += Notify_PaymentFailed;
            notify.UnknownGateway += Notify_UnknownGateway;

            await notify.ReceivedAsync();
        }

        private bool Notify_PaymentSucceed(object sender, PaymentSucceedEventArgs e)
        {
            // 支付成功时时的处理代码
            /* 建议添加以下校验。
             * 1、需要验证该通知数据中的OutTradeNo是否为商户系统中创建的订单号，
             * 2、判断Amount是否确实为该订单的实际金额（即商户订单创建时的金额），
             */
            if (e.GatewayType == typeof(WechatpayGateway))
            {
                var notify = (Notify)e.Notify;
            }
            return true;
        }

        private void Notify_PaymentFailed(object sender, PaymentFailedEventArgs e)
        {
            // 支付失败时的处理代码
        }

        private void Notify_UnknownGateway(object sender, UnknownGatewayEventArgs e)
        {
            // 无法识别支付网关时的处理代码
        }
    }
}