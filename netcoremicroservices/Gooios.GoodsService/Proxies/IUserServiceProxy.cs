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
    public interface IUserServiceProxy
    {
        Task<bool> SetServicerIdForUser(string userName, string servicerId);

        Task<CookAppUserDTO> GetCookAppUser(string userName);
    }

    public class UserServiceProxy : IUserServiceProxy
    {
        public UserServiceProxy() { }

        public async Task<CookAppUserDTO> GetCookAppUser(string idOrUserName)
        {
            CookAppUserDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "63e960be918689e799162326c7e9f189");

                    var reqUrl = $"http://userservice.gooios.com/api/CookAppUser/v1?idOrUserName={idOrUserName}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<CookAppUserDTO>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<bool> SetServicerIdForUser(string userName, string servicerId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "63e960be918689e799162326c7e9f189");

                    var reqUrl = $"http://userservice.gooios.com/api/cookappuser/setservicerid/v1";
                    var jsonObj = JsonConvert.SerializeObject(new { UserName = userName, ServicerId = servicerId });
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                    var res = await client.PostAsync(reqUrl, content);
                    if (res.IsSuccessStatusCode)
                    {
                        var c = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<bool>(c);
                        return result;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                //TODO: add error handle logic
                return false;
            }
        }
    }
}
