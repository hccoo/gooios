using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class GoodsTagStatisticsDTO
    {
        public string GoodsId { get; set; }

        public List<TagStatisticsDTO> TagStatistics { get; set; }
    }

    public class TagStatisticsDTO
    {
        public string TagId { get; set; }

        public string TagName { get; set; }

        public int Count { get; set; }
    }
}
