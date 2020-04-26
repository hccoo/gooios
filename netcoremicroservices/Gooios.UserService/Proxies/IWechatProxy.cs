using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.UserService.Proxies
{
    public interface IWechatProxy
    {
        Task<GetAccessTokenResponseModel> GetAccessToken(string appId, string secret, string code, string grantType);

        Task<WeChatOpenIdResponseDto> CheckAuthCode(WeChatOpenIdRequestDto model);
    }

    public class WechatProxy : IWechatProxy
    {
        public async Task<GetAccessTokenResponseModel> GetAccessToken(string appId, string secret, string code, string grantType)
        {
            //https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code

            var url = "https://api.weixin.qq.com/sns/oauth2/access_token";
            var parameters = new Dictionary<string,string>{
                { "appid",appId },
                { "secret",secret},
                { "code",code},
                { "grant_type",grantType}
            };
            var result = await GetAsync<GetAccessTokenResponseModel>(url, parameters, "");
            return result;
        }


        public async Task<WeChatOpenIdResponseDto> CheckAuthCode(WeChatOpenIdRequestDto model)
        {
            WeChatOpenIdResponseDto result = null;

            var api = $"https://api.weixin.qq.com/sns/jscode2session?appid={model.AppId}&secret={model.Secret}&js_code={model.Code}&grant_type={model.GrantType}";

            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(api);

                if (res.IsSuccessStatusCode)
                {
                    var resultStr = await res.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<WeChatOpenIdResponseDto>(resultStr);
                }
            }

            //TODO:验签

            return result;
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
                if(!string.IsNullOrEmpty(apiKey)) client.DefaultRequestHeaders.Add("gooiosapikey", apiKey);

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

    public class GetAccessTokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("openid")]
        public string OpenId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("unionid")]
        public string UnionId { get; set; }
    }

    public class ErrorResponseModel
    {
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
    }


    public class WeChatOpenIdRequestDto
    {
        public string AppId { get; set; }

        public string Secret { get; set; }

        public string GrantType { get; set; } = "authorization_code";

        public string Code { get; set; }
    }
    public class WeChatOpenIdResponseDto
    {
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        [JsonProperty("session_key")]
        public string SessionKey { get; set; }

        [JsonProperty("errcode")]
        public int ErrCode { get; set; } = 0;

        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; } = string.Empty;

        [JsonProperty("unionid")]
        public string UnionId { get; set; }
    }
}
