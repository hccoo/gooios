using Gooios.UserService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Domain.Aggregates
{
    public class CookAppPartnerLoginUser : Entity<string>
    {
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

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class CookAppPartnerLoginUserFactory
    {
        public static CookAppPartnerLoginUser CreateInstance(string authCode, string accessToken, int expiredIn, string refreshToken, string scope, string unionId, LoginChannel loginChannel, string partnerKey)
        {
            var now = DateTime.Now;
            var obj = new CookAppPartnerLoginUser
            {
                CreatedBy = partnerKey,
                CreatedOn = now,
                ExpiredIn = expiredIn,
                LoginChannel = loginChannel,
                PartnerAccessToken = accessToken,
                PartnerAuthCode = authCode,
                PartnerKey = partnerKey,
                RefreshToken = refreshToken,
                Scope = scope,
                UnionId = unionId,
                UpdatedBy = partnerKey,
                UpdatedOn = now
            };
            obj.GenerateId();

            return obj;
        }
    }

    public enum LoginChannel
    {
        Wechat = 1,
        Alipay = 2
    }
}
