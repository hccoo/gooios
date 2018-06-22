using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.DTOs
{
    public class ServiceTagStatisticsDTO
    {
        public string ServiceId { get; set; }

        public List<TagStatisticsDTO> TagStatistics { get; set; }
    }

    public class ServicerTagStatisticsDTO
    {
        public string ServicerId { get; set; }

        public List<TagStatisticsDTO> TagStatistics { get; set; }
    }

    public class TagStatisticsDTO
    {
        public string TagId { get; set; }

        public string TagName { get; set; }

        public int Count { get; set; }
    }
}
