using Gooios.PartnerGatewayService.Applications.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.PartnerGateway.Proxies
{
    public interface IPaymentServiceProxy
    {
        Task WeChatPaymentNotify(WeChatPaymentNotifyMessageDTO model);
    }
    public class PaymentServiceProxy : IPaymentServiceProxy
    {
        public async Task WeChatPaymentNotify(WeChatPaymentNotifyMessageDTO model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "896960bff18111e799160126c7e9f007");

                var reqUrl = $"http://paymentservice.gooios.com/api/wechatpaymentnotify/v1";

                var data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(reqUrl, content);
            }
        }
    }
}
