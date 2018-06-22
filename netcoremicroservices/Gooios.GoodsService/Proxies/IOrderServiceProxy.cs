using Gooios.GoodsService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Proxies
{
    public interface IOrderServiceProxy
    {
        Task<OrderDTO> CreateOrder(OrderDTO model);
    }

    public class OrderServiceProxy : IOrderServiceProxy
    {
        public async Task<OrderDTO> CreateOrder(OrderDTO model)
        {
            OrderDTO result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                    var reqUrl = $"http://orderservice.gooios.com/api/order/v1";
                    var jsonObj = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                    var res = await client.PostAsync(reqUrl, content);
                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<OrderDTO>(resultStr);
                    }
                }
            }
            catch(Exception ex)
            {
                //TODO: add error handle logic
            }
            return result;
        }
    }
}
