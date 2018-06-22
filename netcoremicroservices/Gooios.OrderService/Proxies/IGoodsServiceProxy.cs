using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.OrderService.Proxies
{
    public interface IGoodsServiceProxy
    {
        Task SetStock(string goodsId, int increment);
    }

    public class GoodsServiceProxy : IGoodsServiceProxy
    {
        public async Task SetStock(string goodsId, int increment)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "63e960bff18111e79916012cc8e9f005");

                var reqUrl = $"http://goodsservice.gooios.com/api/goods/v1/modifystock?goodsid={goodsId}&increment={increment}";
             
                await client.PutAsync(reqUrl,null);
            }
        }
    }
}
