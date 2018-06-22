using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.Infrastructure.Logs
{
    public class LogProvider
    {
        public static async Task Trace(Log log)
        {
            try
            {
                var api = "http://logservice.gooios.com/api/log/v1";
                var gooiosApiKey = "63e960bff18111e799168926c7e9f005";

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(log);
                await PostAsync(api, json, gooiosApiKey);
            }
            catch(Exception ex) {
                //Console.WriteLine($"ERROR:{ex.Message}");
            }
        }

        static async Task<T> GetAsync<T>(string url, Dictionary<string, string> param, string apiKey)
            where T : class
        {
            T ret = default(T);

            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            var p = string.Join("&", param.Select(o => { return o.Key + "=" + o.Value; }));
            url = string.Join("?", url, p);

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", apiKey);
                var response = await client.GetAsync(url);//await异步等待回应
                response.EnsureSuccessStatusCode(); //确保HTTP成功状态值
                var res = (await response.Content.ReadAsStringAsync());//await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        static async Task PostAsync(string url, string jsonStr, string apiKey)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", apiKey);
                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var postResult = await client.PostAsync(url, content);
                var readResult = await postResult.Content.ReadAsStringAsync();

                //Console.WriteLine($"Status:{postResult.StatusCode}");
            }
        }
    }
}
