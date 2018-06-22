using Gooios.GoodsService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public class OnlineGrouponCondition : Entity<int>
    {
        public string GoodsId { get; set; }

        public int MoreThanNumber { get; set; }

        public decimal Price { get; set; }
    }
}
