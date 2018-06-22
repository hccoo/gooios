using Gooios.PaymentService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies
{
    public interface IOrderServiceProxy
    {
        Task SetOrderPaidSuccessed(string orderId);

        Task SetOrderPaidFailed(string orderId);

        Task<OrderDTO> GetById(string orderId);

        Task<OrderDTO> GetByOrderNo(string orderNo);
    }

    public class OrderServiceProxy : IOrderServiceProxy
    {
        public async Task SetOrderPaidSuccessed(string orderId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                var reqUrl = $"http://orderservice.gooios.com/api/order/v1/paidsuccess?orderId={orderId}";

                await client.PutAsync(reqUrl, null);
            }

        }

        public async Task SetOrderPaidFailed(string orderId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                var reqUrl = $"http://orderservice.gooios.com/api/order/v1/paidfailed?orderId={orderId}";

                await client.PutAsync(reqUrl, null);
            }

        }

        public async Task<OrderDTO> GetById(string orderId)
        {
            OrderDTO result = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                var reqUrl = $"http://orderservice.gooios.com/api/order/v1/orderid/{orderId}";

                var res = await client.GetAsync(reqUrl);

                if (res.IsSuccessStatusCode)
                {
                    var retStr = await res.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrderDTO>(retStr);
                }
            }
            return result;
        }

        public async Task<OrderDTO> GetByOrderNo(string orderNo)
        {
            OrderDTO result = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "77e960be918111e709189226c7e9f002");

                var reqUrl = $"http://orderservice.gooios.com/api/order/v1/orderno/{orderNo}";

                var res = await client.GetAsync(reqUrl);

                if (res.IsSuccessStatusCode)
                {
                    var retStr = await res.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OrderDTO>(retStr);
                }
            }
            return result;
        }
    }
}
