using System;
using System.Collections.Generic;
using System.Text;

namespace Gooios.UserService.Proxies.Dtos
{
    public class CookAppPartnerLoginUserDto
    {
        public string Id { get; set; }

        public string PartnerAuthCode { get; set; }

        public string PartnerAccessToken { get; set; }

        public int ExpiredIn { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public string UnionId { get; set; }

        public LoginChannel LoginChannel { get; set; }

        /// <summary>
        /// 类似Partner的openid等
        /// </summary>
        public string PartnerKey { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public enum LoginChannel
    {
        Wechat = 1,
        Alipay = 2
    }
}
