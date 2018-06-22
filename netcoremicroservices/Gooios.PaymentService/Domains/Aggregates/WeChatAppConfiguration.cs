using Gooios.PaymentService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Domains.Aggregates
{
    public class WeChatAppConfiguration : Entity<string>
    {
        public string AppId { get; set; }

        public string MchId { get; set; }

        public string Key { get; set; }

        public string AppSecret { get; set; }

        public string SslCertPath { get; set; }

        public string SslCertPassword { get; set; }

        public string NotifyUrl { get; set; }

        public string OrganizationId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
