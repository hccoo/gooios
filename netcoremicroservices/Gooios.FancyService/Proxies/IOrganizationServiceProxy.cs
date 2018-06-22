using Gooios.FancyService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies
{
    public interface IOrganizationServiceProxy
    {
        Task<OrganizationDTO> GetOrganizationById(string id);
    }

    public class OrganizationServiceProxy : IOrganizationServiceProxy
    {
        public async Task<OrganizationDTO> GetOrganizationById(string id)
        {
            OrganizationDTO result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "999960bff18111e799160126c7e9f568");

                    var reqUrl = $"http://organizationservice.gooios.com/api/organization/v1/getbyid?id={id}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<OrganizationDTO>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: add error handle logic
            }

            return result;
        }
    }
}
