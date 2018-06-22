using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Repositories
{
    public class GrouponParticipationRepository : Repository<GrouponParticipation, int>, IGrouponParticipationRepository
    {
        public GrouponParticipationRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
