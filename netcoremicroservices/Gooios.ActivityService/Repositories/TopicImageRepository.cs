using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Repositories
{
    public class TopicImageRepository : Repository<TopicImage, int>, ITopicImageRepository
    {
        public TopicImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
