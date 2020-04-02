using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models
{
    public class PartnerLogin
    {
        public string Id { get; set; }
        public string OpenId { get; set; }

        public string AuthorizationCode { get; set; }

        public string AccessToken { get; set; }

        public int ExpiredIn { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public string UnionId { get; set; }

        public LoginChannel LoginChannel { get; set; }

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
