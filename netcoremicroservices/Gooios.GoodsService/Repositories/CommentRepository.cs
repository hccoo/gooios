using Gooios.GoodsService.Domains;
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Repositories
{
    public class CommentRepository : Repository<Comment, string>, ICommentRepository
    {
        public CommentRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
