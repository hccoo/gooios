using Gooios.PaymentService.Domains.Aggregates;
using Gooios.PaymentService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Repositories
{
    public class WeChatPaymentNotifyMessageRepository : Repository<WeChatPaymentNotifyMessage, int>, IWeChatPaymentNotifyMessageRepository
    {
        public WeChatPaymentNotifyMessageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
