using Gooios.GoodsService.Domains;
using Gooios.GoodsService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Repositories
{
    public interface ITagRepository : IRepository<Tag, string>
    {
        IEnumerable<Tag> GetByGoodsId(string goodsId);
    }
}
