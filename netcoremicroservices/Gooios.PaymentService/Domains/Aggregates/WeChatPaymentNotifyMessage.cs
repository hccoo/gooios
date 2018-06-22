using Gooios.PaymentService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Domains.Aggregates
{
    public class WeChatPaymentNotifyMessage : Entity<int>
    {
        public string NotifyApiName { get; set; }

        /// <summary>
        /// WeCahtPaymentNotifyMessageDTO 的Json内容
        /// </summary>
        public string MessageContent { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
