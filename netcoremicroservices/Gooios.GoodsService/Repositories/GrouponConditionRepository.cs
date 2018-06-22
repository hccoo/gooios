using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Repositories
{
    public class GrouponConditionRepository : Repository<GrouponCondition, int>, IGrouponConditionRepository
    {
        public GrouponConditionRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
