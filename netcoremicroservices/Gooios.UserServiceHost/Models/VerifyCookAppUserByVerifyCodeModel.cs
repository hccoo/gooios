using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserServiceHost.Models
{
    public class VerifyCookAppUserByVerifyCodeModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
