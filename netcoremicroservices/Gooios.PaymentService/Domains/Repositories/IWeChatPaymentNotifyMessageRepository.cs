using Gooios.PaymentService.Domain;
using Gooios.PaymentService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Domains.Repositories
{
    public interface IWeChatPaymentNotifyMessageRepository : IRepository<WeChatPaymentNotifyMessage, int>
    {
    }
}
