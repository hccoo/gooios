using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Domains.Events
{
    public class GrouponParticipatedEvent : DomainEvent
    {
        public GrouponParticipatedEvent() { }
        public GrouponParticipatedEvent(IEntity source) : base(source) { }

        public DateTime ParticipateTime { get; set; }
    }

    public class GrouponParticipatedEventHandler : IDomainEventHandler<GrouponParticipatedEvent>
    {
        private readonly IEventBus _bus;
        private readonly IDbUnitOfWork _dbUnitOfWork;
        private readonly IGrouponActivityRepository _grouponActivityRepository;
        private readonly IGrouponParticipationRepository _grouponParticipationRepository;

        public GrouponParticipatedEventHandler(IEventBus bus, 
            IGrouponActivityRepository grouponActivityRepository,
            IGrouponParticipationRepository grouponParticipationRepository,
            IDbUnitOfWork dbUnitOfWork
            )
        {
            _bus = bus;
            _grouponActivityRepository = grouponActivityRepository;
            _grouponParticipationRepository = grouponParticipationRepository;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void Handle(GrouponParticipatedEvent evnt)
        {
            var eventSource = evnt.Source as GrouponParticipation;

            var activity = _grouponActivityRepository.Get(eventSource.GrouponActivityId);

            if (activity == null) throw new Exception("找不到指定的活动记录。");

            var buyCount = _grouponParticipationRepository.GetFiltered(o => o.GrouponActivityId == eventSource.GrouponActivityId).Sum(g => g.BuyCount);


            if(buyCount>= activity.Count)
            {
                activity.SetCompleted();
            }

            _grouponActivityRepository.Update(activity);
            _dbUnitOfWork.Commit();

            _bus.Publish(evnt);
        }
    }
}
