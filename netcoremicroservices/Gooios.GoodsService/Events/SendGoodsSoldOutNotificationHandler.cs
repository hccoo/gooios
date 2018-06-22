using Gooios.Infrastructure.Events;
using Gooios.GoodsService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Events
{
    [HandlesAsynchronously]
    public class SendGoodsSoldOutNotificationHandler : IEventHandler<GoodsSoldOutEvent>
    {
        public SendGoodsSoldOutNotificationHandler()
        {
        }

        public void Handle(GoodsSoldOutEvent evnt)
        {
            // Send sms logic
        }
    }
}
