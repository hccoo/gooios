using Gooios.ActivityService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Domains.Aggregates
{
    public class GrouponParticipation : Entity<int>
    {
        public string GrouponActivityId { get; set; }

        public string UserId { get; set; }

        public int BuyCount { get; set; }

        public DateTime ParticipateTime { get; set; }

        public string OrderId { get; set; }

        public void ConfirmParticipated()
        {
            DomainEvent.Publish<GrouponParticipatedEvent>(new GrouponParticipatedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, ParticipateTime = DateTime.Now });
        }
    }
}
