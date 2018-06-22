using Gooios.FancyService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies
{
    public interface IAuthServiceProxy
    {
        Task<ApplicationUserDTO> GetUser(string userId);
    }

    public class AuthServiceProxy : IAuthServiceProxy
    {
        public async Task<ApplicationUserDTO> GetUser(string userId)
        {
            ApplicationUserDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "6de960be9183367800160026c7e9f3d2");

                    var reqUrl = $"https://authservice.gooios.com/api/user?userid={userId}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<ApplicationUserDTO>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}
