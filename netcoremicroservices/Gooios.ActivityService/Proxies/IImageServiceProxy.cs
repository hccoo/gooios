using Gooios.ActivityService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Proxies
{
    public interface IImageServiceProxy
    {
        Task<IEnumerable<ImageDTO>> GetImagesByIds(IEnumerable<string> ids);

        Task<ImageDTO> GetImageById(string id);
    }

    public class ImageServiceProxy : IImageServiceProxy
    {
        public async Task<ImageDTO> GetImageById(string id)
        {
            ImageDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "63e960bff18111e799160126c7e9f004");

                    var reqUrl = $"https://imageservice.gooios.com/api/image/v1/{id}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ImageDTO>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<IEnumerable<ImageDTO>> GetImagesByIds(IEnumerable<string> ids)
        {
            IEnumerable<ImageDTO> result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "63e960bff18111e799160126c7e9f004");
                    var idsStr = string.Join(',', ids);

                    var reqUrl = $"https://imageservice.gooios.com/api/image/v1/images?ids={idsStr}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<IEnumerable<ImageDTO>>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                result = new List<ImageDTO>();
            }

            return result;
        }
    }
}
