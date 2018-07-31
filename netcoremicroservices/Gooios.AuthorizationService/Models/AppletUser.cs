using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models
{
    public class AppletUser
    {
        public string Id { get; set; }

        public string OpenId { get; set; }

        public UserChannel Channel { get; set; } = UserChannel.WeChat;

        public string NickName { get; set; } = string.Empty;

        public string UserPortrait { get; set; } = string.Empty;

        public string OrganizationId { get; set; } = string.Empty;

        public string ApplicationId { get; set; } = string.Empty;

        /// <summary>
        /// AuthService下的用户ID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdOn { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public static class AppletUserFactory
    {
        public static AppletUser CreateInstance(
            string openId,
            string organizationId,
            string appId,
            UserChannel channel = UserChannel.WeChat,
            string nickName = "",
            string userPortrait = "",
            string userId = "")
        {
            var result = new AppletUser
            {
                ApplicationId = appId,
                Channel = channel,
                NickName = nickName,
                OpenId = openId,
                OrganizationId = organizationId,
                UserId = userId,
                UserPortrait = userPortrait,
                CreatedOn = DateTime.Now,
                LastUpdOn = DateTime.Now
            };

            result.GenerateId();

            return result;
        }
    }

    public enum UserChannel
    {
        WeChat = 1,
        AliPay = 2
    }
}
