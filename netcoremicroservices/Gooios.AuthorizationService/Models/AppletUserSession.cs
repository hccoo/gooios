using Gooios.Infrastructure.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models
{
    public class AppletUserSession
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

        public void GenerateGooiosSessionKey()
        {
            GooiosSessionKey = SecretProvider.EncryptToMD5($"{UserId}#{OpenId}${Guid.NewGuid().ToString()}>{CreatedOn.ToString()}");
        }
    }

    public class UserSessionFactory
    {
        public static AppletUserSession CreateInstance(string userId, string openId, string sessionKey)
        {
            var result = new AppletUserSession
            {
                CreatedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddDays(7),
                OpenId = openId,
                SessionKey = sessionKey,
                UserId = userId
            };

            result.GenerateGooiosSessionKey();

            return result;
        }
    }
}
