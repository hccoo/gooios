using Gooios.PaymentService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies
{
    public interface IAuthServiceProxy
    {
        Task AddOrUpdateAppletUser(AppletUserDTO model);

        Task<AppletUserSessionDTO> AddOrUpdateAppletUserSession(AppletUserSessionDTO model);
    }

    public class AuthServiceProxy : IAuthServiceProxy
    {
        public async Task AddOrUpdateAppletUser(AppletUserDTO model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "6de960be9183367800160026c7e9f3d2");

                var reqUrl = $"http://authservice.gooios.com/api/user/appletuser";
                var data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(reqUrl, content);
            }
        }

        public async Task<AppletUserSessionDTO> AddOrUpdateAppletUserSession(AppletUserSessionDTO model)
        {
            AppletUserSessionDTO result = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", "6de960be9183367800160026c7e9f3d2");

                var reqUrl = $"http://authservice.gooios.com/api/user/appletusersession";
                var data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(reqUrl, content);

                if (res.IsSuccessStatusCode)
                {
                    var resContent = await res.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AppletUserSessionDTO>(resContent);
                }
                return result;
            }
        }
    }
}
