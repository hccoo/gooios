using Gooios.Infrastructure.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies.DTOs
{
    public class AppletUserSessionDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// The session key of WeChat or AliPay
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// Custom session key
        /// </summary>
        public string GooiosSessionKey { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiredOn { get; set; }
    }
}
