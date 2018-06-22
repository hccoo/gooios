using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderShippedHandler : IEventHandler<OrderShippedEvent>
    {
        public void Handle(OrderShippedEvent evnt)
        {
            //TODO: Notificate the user
        }
    }
}
