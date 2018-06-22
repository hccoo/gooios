using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderCompletedHandler : IEventHandler<OrderCompletedEvent>
    {
        public void Handle(OrderCompletedEvent evnt)
        {
            //TODO: Notificate the user
        }
    }
}
