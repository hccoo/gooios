using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderRefundedHandler : IEventHandler<OrderRefundedEvent>
    {
        public void Handle(OrderRefundedEvent evnt)
        {
            //TODO: Notificate the user
        }
    }
}
