using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies.DTOs
{
    public class WeChatOpenIdRequestDTO
    {
        public string AppId { get; set; }

        public string Secret { get; set; }

        public string GrantType { get; set; } = "authorization_code";

        public string Code { get; set; }
    }
}
