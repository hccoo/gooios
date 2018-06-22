using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Events
{
    public class GoodsSoldOutEvent : DomainEvent
    {
        public GoodsSoldOutEvent() { }
        public GoodsSoldOutEvent(IEntity source) : base(source) { }

        public DateTime SoldOutTime { get; set; }
    }

    public class GoodsSoldOutEventHandler : IDomainEventHandler<GoodsSoldOutEvent>
    {
        private readonly IEventBus _bus;
        private readonly IOnlineGoodsRepository _onlineGoodsRepository;

        public GoodsSoldOutEventHandler(IEventBus bus,IOnlineGoodsRepository onlineGoodsRepository)
        {
            _bus = bus;
            _onlineGoodsRepository = onlineGoodsRepository;
        }

        public void Handle(GoodsSoldOutEvent evnt)
        {
            var eventSource = evnt.Source as OnlineGoods;
            eventSource.SetSoldOut();
            _onlineGoodsRepository.Update(eventSource);

            _bus.Publish(evnt);
        }
    }
}
