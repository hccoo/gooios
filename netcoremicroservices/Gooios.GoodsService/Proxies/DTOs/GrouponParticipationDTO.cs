using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Proxies.DTOs
{
    public class GrouponParticipationDTO
    {
        public string GrouponActivityId { get; set; }

        public string UserId { get; set; }

        public string NickName { get; set; }

        public string UserPortraitUrl { get; set; }

        public int BuyCount { get; set; }

        public string ParticipateTime { get; set; }

        public string OrderId { get; set; }
    }
}
