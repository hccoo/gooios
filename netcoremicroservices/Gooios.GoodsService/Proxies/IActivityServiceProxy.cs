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
    public interface IActivityServiceProxy
    {
        Task<GrouponActivityDTO> CreateGrouponActivity(GrouponActivityDTO model);

        Task AddGrouponParticipation(GrouponParticipationDTO model);

        Task<GrouponActivityDTO> GetActivityById(string id);
    }

    public class ActivityServiceProxy:IActivityServiceProxy
    {
        public async Task<GrouponActivityDTO> CreateGrouponActivity(GrouponActivityDTO model)
        {
            GrouponActivityDTO result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "83e960bff18221e39916012cc8e9f609");
                    client.DefaultRequestHeaders.Add("userid", model.CreatedBy);

                    var reqUrl = $"http://activityservice.gooios.com/api/GrouponActivity/v1";
                    var jsonObj = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                    var res = await client.PostAsync(reqUrl, content);
                    
                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<GrouponActivityDTO>(resultStr);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: add error handle logic
            }
            return result;
        }

        public async Task AddGrouponParticipation(GrouponParticipationDTO model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "83e960bff18221e39916012cc8e9f609");
                    client.DefaultRequestHeaders.Add("userid", model.UserId);

                    var reqUrl = $"http://activityservice.gooios.com/api/GrouponParticipation/v1";
                    var jsonObj = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                    var res = await client.PostAsync(reqUrl, content);
                }
            }
            catch (Exception ex)
            {
                //TODO: add error handle logic
            }
        }

        public async Task<GrouponActivityDTO> GetActivityById(string id)
        {
            GrouponActivityDTO result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("gooiosapikey", "83e960bff18221e39916012cc8e9f609");

                    var reqUrl = $"http://activityservice.gooios.com/api/GrouponActivity/v1/id/{id}";

                    var res = await client.GetAsync(reqUrl);

                    if (res.IsSuccessStatusCode)
                    {
                        var resultStr = await res.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<GrouponActivityDTO>(resultStr);
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
