using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Repositories
{
    public class CommentImageRepository : Repository<CommentImage, int>, ICommentImageRepository
    {
        public CommentImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
