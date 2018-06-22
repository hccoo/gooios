using Gooios.ActivityService.Configurations;
using Gooios.ActivityService.Proxies.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Proxies
{
    public interface IAmapProxy
    {
        Task<GeoResponseDTO> Geo(string address);
    }

    public class AmapProxy : IAmapProxy
    {
        readonly IServiceConfigurationProxy _serviceConfiguration;

        public AmapProxy(IServiceConfigurationProxy serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        public async Task<GeoResponseDTO> Geo(string address)
        {
            GeoResponseDTO ret = null;
            
            var url = $"{_serviceConfiguration.AmapRootUrl}/v3/geocode/geo?address={address}&key={_serviceConfiguration.AmapKey}";

            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    var resContent = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(resContent);

                    double lon = 0;
                    double lat = 0;

                    if (!string.IsNullOrEmpty(result?.geocodes?[0]?.location?.ToString()))
                    {
                        var arr = result.geocodes[0].location.ToString().Split(',');
                        if (arr.Length == 2)
                        {
                            double.TryParse(arr[0], out lon);
                            double.TryParse(arr[1], out lat);
                        }
                    }

                    ret = new GeoResponseDTO { Latitude = lat, Longitude = lon };
                }
            }
            return ret;
        }
    }
}
