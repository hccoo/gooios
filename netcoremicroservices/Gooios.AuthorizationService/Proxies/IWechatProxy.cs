using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Proxies
{
    public interface IWechatProxy
    {
        Task<GetAccessTokenResponseModel> GetAccessToken(string appId, string secret, string code, string grantType,out ErrorResponseModel res);
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
}
