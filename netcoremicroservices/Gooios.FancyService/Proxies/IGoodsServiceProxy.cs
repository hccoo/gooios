using Gooios.FancyService.Applications.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies
{
    public interface IGoodsServiceProxy
    {
        Task<IEnumerable<GoodsDTO>> GetGoodsByGoodsCategoryName(string goodsCategoryName);
    }

    public class GoodsServiceProxy : IGoodsServiceProxy
    {
        public async Task<IEnumerable<GoodsDTO>> GetGoodsByGoodsCategoryName(string goodsCategoryName)
        {
            IEnumerable<GoodsDTO> result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "63e960bff18111e79916012cc8e9f005");

                    var reqUrl = $"http://goodsservice.gooios.com/api/goods/v1/getgoodsbygoodscategorynames?goodsCategoryName={goodsCategoryName}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<IEnumerable<GoodsDTO>>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
    }
}
