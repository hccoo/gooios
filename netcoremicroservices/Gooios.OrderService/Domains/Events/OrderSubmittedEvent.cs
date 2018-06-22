using Gooios.OrderService.Domains;
using Gooios.OrderService.Domains.Events;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gooios.OrderService.Domains.Repositories;
using Gooios.Infrastructure;
using Gooios.OrderService.Domains.Aggregates;

namespace Gooios.OrderService.Domains.Events
{
    public class OrderSubmittedEvent : DomainEvent
    {
        public OrderSubmittedEvent() { }
        public OrderSubmittedEvent(IEntity source) : base(source) { }

        public DateTime SubmittedTime { get; set; }
    }

    public class OrderSubmittedEventHandler : IDomainEventHandler<OrderSubmittedEvent>
    {
        private readonly IEventBus _bus;
        private readonly IOrderTraceRepository _orderTraceRepository;
        private readonly IDbUnitOfWork _dbUnitOfWork;

        public OrderSubmittedEventHandler(IEventBus bus, IOrderTraceRepository orderTraceRepository, IDbUnitOfWork dbUnitOfWork)
        {
            _orderTraceRepository = orderTraceRepository;
            _bus = bus;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void Handle(OrderSubmittedEvent evnt)
        {
            var eventSource = evnt.Source as Order;

            _orderTraceRepository.Add(new OrderTrace { CreatedBy = eventSource.CreatedBy, CreatedOn = DateTime.Now, IsSuccess = true, OrderId = eventSource.Id, Status = eventSource.Status });
            _dbUnitOfWork.Commit();
            _bus.Publish(evnt);
        }
    }
}
