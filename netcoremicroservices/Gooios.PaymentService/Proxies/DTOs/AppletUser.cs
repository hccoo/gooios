using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies.DTOs
{
    public class AppletUserDTO
    {
        public string Id { get; set; }

        public string OpenId { get; set; }

        public UserChannel Channel { get; set; } = UserChannel.WeChat;

        public string NickName { get; set; } = string.Empty;

        public string UserPortrait { get; set; } = string.Empty;

        public string OrganizationId { get; set; }

        public string ApplicationId { get; set; }

        /// <summary>
        /// AuthService下的用户ID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdOn { get; set; }
    }

    public enum UserChannel
    {
        WeChat = 1,
        AliPay = 2
    }
}
