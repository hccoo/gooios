using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Proxies.DTOs
{
    public class GrouponActivityDTO
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string ProductMark { get; set; } = "Goods";

        public int Count { get; set; }

        public decimal UnitPrice { get; set; }

        public ActivityStatus Status { get; set; }

        public DateTime Start { get; set; }

        public string StartStr => Start.ToString("yyyy-MM-dd HH:mm");

        public DateTime End { get; set; }

        public string EndStr => End.ToString("yyyy-MM-dd HH:mm");

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public IEnumerable<GrouponParticipationDTO> GrouponParticipations { get; set; }

        public int TotalBuyCount => GrouponParticipations?.Sum(o => o.BuyCount) ?? 0;

        public string CreatorName { get; set; }

        public string CreatorPortraitUrl { get; set; }
    }

    public enum ActivityStatus
    {
        InProgress = 1,
        Completed = 2,
        Failed = 4
    }
}
