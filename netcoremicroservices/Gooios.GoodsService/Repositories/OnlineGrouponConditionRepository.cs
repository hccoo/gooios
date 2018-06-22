using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Repositories
{
    public class OnlineGrouponConditionRepository : Repository<OnlineGrouponCondition, int>, IOnlineGrouponConditionRepository
    {
        public OnlineGrouponConditionRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
