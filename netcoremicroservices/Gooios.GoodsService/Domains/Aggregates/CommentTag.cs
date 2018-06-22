using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public class CommentTag:Entity<int>
    {
        public string CommentId { get; set; }

        public string TagId { get; set; }

        public string GoodsId { get; set; }

        public string UserId { get; set; }
    }
}
