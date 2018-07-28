using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models.WeChatModels
{
    public class WeChatOpenIdResponseDTO
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
