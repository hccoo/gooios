using Gooios.Infrastructure.Events;
using Gooios.GoodsService.Domains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Events
{
    [HandlesAsynchronously]
    public class SendGoodsShelvedNotificationHandler : IEventHandler<GoodsShelvedEvent>
    {
        public SendGoodsShelvedNotificationHandler()
        {
        }

        public void Handle(GoodsShelvedEvent evnt)
        {
            // Send sms logic
        }
    }
}
