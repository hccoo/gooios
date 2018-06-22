﻿using Gooios.ActivityService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Domains.Repositories
{
    public interface ITopicImageRepository : IRepository<TopicImage, int>
    {
    }
}
