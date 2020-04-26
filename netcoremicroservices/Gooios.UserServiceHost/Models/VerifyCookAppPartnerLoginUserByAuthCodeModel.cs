using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserServiceHost.Models
{
    public class VerifyCookAppPartnerLoginUserByAuthCodeModel
    {
        //[JsonProperty("authCode")]
        public string AuthorizationCode { get; set; }
    }
}
