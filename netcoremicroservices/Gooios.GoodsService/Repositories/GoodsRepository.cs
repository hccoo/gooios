using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Repositories
{
    public class GoodsRepository : Repository<Goods, string>, IGoodsRepository
    {
        public GoodsRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
