using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Events;
using Gooios.OrderService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            IocProvider.GetService<IEventBus>().Subscribe<OrderCancelledEvent>(new OrderCancelledHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderCompletedEvent>(new OrderCompletedHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderPaidEvent>(new OrderPaidHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderPaidFailedEvent>(new OrderPaidFailedHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderRefundedEvent>(new OrderRefundedHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderShippedEvent>(new OrderShippedHandler());
            IocProvider.GetService<IEventBus>().Subscribe<OrderSubmittedEvent>(new OrderSubmittedHandler());
        }
    }
}
