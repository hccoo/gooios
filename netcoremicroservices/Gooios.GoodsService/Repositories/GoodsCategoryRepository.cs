using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Repositories
{
    public class GoodsCategoryRepository: Repository<GoodsCategory, string>, IGoodsCategoryRepository
    {
        public GoodsCategoryRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
