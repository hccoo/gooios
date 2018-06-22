using Gooios.GoodsService.Domains;
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Repositories
{
    public class TagRepository : Repository<Tag, string>, ITagRepository
    {
        public TagRepository(IDbContextProvider provider) : base(provider)
        {

        }

        public IEnumerable<Tag> GetByGoodsId(string goodsId)
        {
            var q = DataContext.Tags.FromSql($" select * from tags where category_id=(select id from goods_categories where `name`=(select ifnull(sub_category,category) from goods where id =@goodsid))  ", new MySqlParameter("goodsid", goodsId));
            return q.ToList();
        }
    }
}
