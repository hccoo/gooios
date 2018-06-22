using Gooios.GoodsService.Domains;
using System;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public class OnlineGoodsImage : Entity<int>
    {
        public string GoodsId { get; set; }

        public string ImageId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
