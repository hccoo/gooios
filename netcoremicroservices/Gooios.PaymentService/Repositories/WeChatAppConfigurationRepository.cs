using Gooios.PaymentService.Domains.Aggregates;
using Gooios.PaymentService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Repositories
{
    public class WeChatAppConfigurationRepository : Repository<WeChatAppConfiguration, string>, IWeChatAppConfigurationRepository
    {
        public WeChatAppConfigurationRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
