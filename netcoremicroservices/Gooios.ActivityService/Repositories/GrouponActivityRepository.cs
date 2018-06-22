using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using Gooios.ActivityService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Repositories
{
    public class GrouponActivityRepository : Repository<GrouponActivity, string>, IGrouponActivityRepository
    {
        public GrouponActivityRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
