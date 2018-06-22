using Gooios.GoodsService.Domains;
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Repositories
{
    public class CommentTagRepository : Repository<CommentTag, int>, ICommentTagRepository
    {
        public CommentTagRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
