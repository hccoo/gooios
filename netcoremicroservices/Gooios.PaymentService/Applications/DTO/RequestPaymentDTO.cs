using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Applications.DTO
{
    public class RequestPaymentRequestDTO
    {
        public double Amount { get; set; }

        public string ProductDescription { get; set; }

        public string ProductDetail { get; set; }

        public string OrderNo { get; set; }

        public string OrderId { get; set; }

        /// <summary>
        /// GoodsId或（ReservationId or ServiceId）
        /// </summary>
        public string ProductId { get; set; }    

        public string TradeType { get; set; } = "JSAPI";

        public string FeeType { get; set; } = "CNY";

        public string OpenId { get; set; }

        public PayChannel PayChannel { get; set; } = PayChannel.AppletPay;
    }

    public class RequestPaymentResponseDTO
    {
        public string TimeStamp { get; set; }

        public string NonceStr { get; set; }

        public string Package { get; set; }

        public string PaySign { get; set; }

        public string AppId { get; set; }

        public string PartnerId { get; set; }

        public string PrepayId { get; set; }

    }

    public class OpenIDSessionKeyDTO
    {
        public string OpenId { get; set; }

        public string SessionKey { get; set; }

        public string GooiosSessionKey { get; set; }
    }
    
    public enum PayChannel
    {
        AppletPay = 1,
        AppPay = 2
    }
}
