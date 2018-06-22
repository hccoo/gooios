using Gooios.AuthorizationService.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Proxies
{
    public interface IVerificationProxy
    {
        Task<VerificationDTO> GetVerification(BizCode bizCode, string to);

        Task SetVerificationUsed(VerificationDTO model);
    }

    public class VerificationProxy : IVerificationProxy
    {
        readonly IServiceConfigurationProxy _configuration;

        public VerificationProxy(IServiceConfigurationProxy config)
        {
            _configuration = config;
        }

        public async Task<VerificationDTO> GetVerification(BizCode bizCode, string to)
        {
            var parameters = new Dictionary<string, string>();
            return await GetAsync<VerificationDTO>(_configuration.VerificationServiceRootUrl+ "api/verification/v1/"+bizCode.ToString()+"/"+to, parameters,_configuration.VerificationServiceHeaderValue);
        }

        public async Task SetVerificationUsed(VerificationDTO model)
        {
            var jsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            await PutAsync(_configuration.VerificationServiceRootUrl + "api/verification/v1", jsonObj, _configuration.VerificationServiceHeaderValue);
        }

        async Task<T> GetAsync<T>(string url, Dictionary<string, string> param, string apiKey)
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

        async Task PutAsync(string url, string jsonStr, string apiKey)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", apiKey);
                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var postResult = await client.PutAsync(url, content);
                var readResult = await postResult.Content.ReadAsStringAsync();
            }
        }
    }

    public class VerificationDTO
    {
        public string Code { get; set; }

        public string To { get; set; }

        public BizCode BizCode { get; set; }
    }

    public enum BizCode
    {
        Register = 1,
        ForgetPassword = 2
    }
}
