using Gooios.FancyService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies
{
    public interface IOrderServiceProxy
    {
        Task CreateOrder(OrderDTO model);
    }

    public class OrderServiceProxy : IOrderServiceProxy
    {
        public async Task CreateOrder(OrderDTO model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                    var reqUrl = $"http://orderservice.gooios.com/api/order/v1";
                    var jsonObj = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                    var res = await client.PostAsync(reqUrl, content);
                }
            }
            catch(Exception ex)
            {
                //TODO: add error handle logic
            }
        }
    }
}
