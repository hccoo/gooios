using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderCancelledHandler : IEventHandler<OrderCancelledEvent>
    {
        public void Handle(OrderCancelledEvent evnt)
        {
            //TODO: Notificate the user
        }
    }
}
