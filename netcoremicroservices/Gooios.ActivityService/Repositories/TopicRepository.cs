using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Repositories
{
    public class TopicRepository : Repository<Topic, string>, ITopicRepository
    {
        public TopicRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
