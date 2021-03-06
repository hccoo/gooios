﻿using Gooios.OrderService.Domains;
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
    public class OrderPaidFailedEvent : DomainEvent
    {
        public OrderPaidFailedEvent() { }
        public OrderPaidFailedEvent(IEntity source) : base(source) { }

        public DateTime FailedTime { get; set; }
    }

    public class OrderPaidFailedEventHandler : IDomainEventHandler<OrderPaidFailedEvent>
    {
        private readonly IEventBus _bus;
        private readonly IOrderTraceRepository _orderTraceRepository;
        private readonly IDbUnitOfWork _dbUnitOfWork;

        public OrderPaidFailedEventHandler(IEventBus bus, IOrderTraceRepository orderTraceRepository, IDbUnitOfWork dbUnitOfWork)
        {
            _orderTraceRepository = orderTraceRepository;
            _bus = bus;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void Handle(OrderPaidFailedEvent evnt)
        {
            var eventSource = evnt.Source as Order;

            _orderTraceRepository.Add(new OrderTrace { CreatedBy = eventSource.CreatedBy, CreatedOn = DateTime.Now, IsSuccess = true, OrderId = eventSource.Id, Status = eventSource.Status });
            _dbUnitOfWork.Commit();
            _bus.Publish(evnt);
        }
    }
}
