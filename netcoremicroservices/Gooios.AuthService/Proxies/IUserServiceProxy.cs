using Gooios.UserService.Configurations;
using Gooios.UserService.Proxies.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gooios.AuthService.Proxies
{
    public interface IUserServiceProxy
    {
        Task<CookAppUserDto> VerifyCookAppUserByPassword(string userName, string password);

        Task<CookAppUserDto> VerifyCookAppUserByVerifyCode(string phone, string code);

        Task<CookAppPartnerLoginUserDto> VerifyCookAppPartnerLoginUserByAuthCode(string authCode);

        Task<CookAppPartnerLoginUserDto> VerifyWechatAppletLoginUserByCode(string code);

        Task<string> AddCookAppUser(string userName, string password, string mobile, string email);
    }

    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceConfigurationAgent _config;
        
        public UserServiceProxy(IHttpClientFactory clientFactory, IServiceConfigurationAgent config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        public async Task<string> AddCookAppUser(string userName, string password, string mobile, string email)
        {
            string result = null;

            var client = _clientFactory.CreateClient("zk-backend");
            var jsonStr = JsonConvert.SerializeObject(new { userName, password, mobile, email });
            var content = new StringContent(jsonStr);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//x-www-form-urlencoded
            content.Headers.Add("gooiosapikey", _config.UserServiceHeaderValue);

            var response = await client.PostAsync($"{_config.UserServiceRootUrl}api/cookappuser/v1", content);
            var contentJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<string>(contentJson);
            else
                result = "";

            return result;
        }

        public async Task<CookAppPartnerLoginUserDto> VerifyCookAppPartnerLoginUserByAuthCode(string authCode)
        {
            CookAppPartnerLoginUserDto result = null;

            var client = _clientFactory.CreateClient("zk-backend");
            var jsonStr = JsonConvert.SerializeObject(new VerifyCookAppPartnerLoginUserByAuthCodeModel { AuthorizationCode = authCode });
            var content = new StringContent(jsonStr);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//x-www-form-urlencoded
            content.Headers.Add("gooiosapikey", _config.UserServiceHeaderValue);

            var response = await client.PostAsync($"{_config.UserServiceRootUrl}api/cookappuser/verifybyauthcode/v1", content);
            var contentJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<CookAppPartnerLoginUserDto>(contentJson);
            else
                result = null;

            return result;
        }

        public async Task<CookAppUserDto> VerifyCookAppUserByPassword(string userName, string password)
        {
            CookAppUserDto result = null;

            var client = _clientFactory.CreateClient("zk-backend");
            var jsonStr = JsonConvert.SerializeObject(new { userName, password });
            var content = new StringContent(jsonStr);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//x-www-form-urlencoded
            content.Headers.Add("gooiosapikey", _config.UserServiceHeaderValue);

            var response = await client.PostAsync($"{_config.UserServiceRootUrl}api/cookappuser/verifybypassword/v1", content);
            var contentJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<CookAppUserDto>(contentJson);
            else
                result = null;

            return result;
        }

        public async Task<CookAppUserDto> VerifyCookAppUserByVerifyCode(string phone, string code)
        {
            CookAppUserDto result = null;

            var client = _clientFactory.CreateClient("zk-backend");
            var jsonStr = JsonConvert.SerializeObject(new { userName=phone, code });
            var content = new StringContent(jsonStr);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//x-www-form-urlencoded
            content.Headers.Add("gooiosapikey", _config.UserServiceHeaderValue);

            var response = await client.PostAsync($"{_config.UserServiceRootUrl}api/cookappuser/verifybycode/v1", content);
            var contentJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<CookAppUserDto>(contentJson);
            else
                result = null;

            return result;
        }

        public async Task<CookAppPartnerLoginUserDto> VerifyWechatAppletLoginUserByCode(string code)
        {
            CookAppPartnerLoginUserDto result = null;

            var client = _clientFactory.CreateClient("zk-backend");
            var jsonStr = JsonConvert.SerializeObject(new VerifyCookAppPartnerLoginUserByAuthCodeModel { AuthorizationCode = code });
            var content = new StringContent(jsonStr);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//x-www-form-urlencoded
            content.Headers.Add("gooiosapikey", _config.UserServiceHeaderValue);

            var response = await client.PostAsync($"{_config.UserServiceRootUrl}api/cookappuser/verifybywechatapplet/v1", content);
            var contentJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<CookAppPartnerLoginUserDto>(contentJson);
            else
                result = null;

            return result;
        }
    }


    public class VerifyCookAppPartnerLoginUserByAuthCodeModel
    {
        //[JsonProperty("authCode")]
        public string AuthorizationCode { get; set; }
    }
}
