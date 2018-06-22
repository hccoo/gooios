using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class GoodsImageDTO
    {
        public int Id { get; set; }

        public string GoodsId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
