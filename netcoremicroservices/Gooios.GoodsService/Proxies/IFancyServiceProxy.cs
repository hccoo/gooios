using Gooios.GoodsService.Applications.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Proxies
{
    public interface IFancyServiceProxy
    {
        Task<ServicerDTO> GetServicer(string userId);
    }

    public class FancyServiceProxy : IFancyServiceProxy
    {
        public async Task<ServicerDTO> GetServicer(string id)
        {
            ServicerDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "768960bff18111e79916016898e9f885");

                    var reqUrl = $"http://fancyservice.gooios.com/api/servicer/v1/getbyid?id={id}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ServicerDTO>(resultStr);
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
