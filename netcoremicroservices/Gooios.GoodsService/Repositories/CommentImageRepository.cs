using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Repositories
{
    public class CommentImageRepository : Repository<CommentImage, int>, ICommentImageRepository
    {
        public CommentImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
