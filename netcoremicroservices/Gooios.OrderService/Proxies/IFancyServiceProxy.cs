using Gooios.Infrastructure.Logs;
using Gooios.OrderService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.OrderService.Proxies
{
    public interface IFancyServiceProxy
    {
        Task SetOrderId(ReservationDTO model);
    }

    public class FancyServiceProxy : IFancyServiceProxy
    {
        public async Task SetOrderId(ReservationDTO model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "768960bff18111e79916016898e9f885");

                var reqUrl = $"http://fancyservice.gooios.com/api/reservation/v1/setorderid";

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                
                var res = await client.PutAsync(reqUrl, content);

                LogProvider.Trace(new Log
                {
                    ApplicationKey = "77e960be918111e709189226c7e9f002",
                    AppServiceName = "OrderService",
                    BizData = res.StatusCode.ToString(),
                    CallerApplicationKey = "",
                    Exception = "",
                    Level = LogLevel.INFO,
                    LogThread = -1,
                    LogTime = DateTime.Now,
                    Operation = "OrderSubmittedHandler",
                    ReturnValue = ""
                });
            }
        }
    }
}
