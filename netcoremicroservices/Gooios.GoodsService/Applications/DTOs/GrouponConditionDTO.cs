using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class GrouponConditionDTO
    {
        public int Id { get; set; }

        public string GoodsId { get; set; }

        public int MoreThanNumber { get; set; }

        public decimal Price { get; set; }
    }
}
