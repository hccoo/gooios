using Gooios.PaymentService.Applications.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Proxies
{
    public interface IPaymentServiceProxy
    {
        Task<OpenIDSessionKeyDTO> InitSession(InitSessionModel model);
    }

    public class PaymentServiceProxy : IPaymentServiceProxy
    {
        public async Task<OpenIDSessionKeyDTO> InitSession(InitSessionModel model)
        {
            OpenIDSessionKeyDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "896960bff18111e799160126c7e9f007");

                    var reqUrl = $"http://paymentservice.gooios.com/api/wechatpayment/v1/session";

                    var res = await client.PostAsync(reqUrl,new StringContent(JsonConvert.SerializeObject(model)));

                    if (res.IsSuccessStatusCode)
                    {
                        var retStr = await res.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<OpenIDSessionKeyDTO>(retStr);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
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
