using Gooios.GoodsService.Domains.Events;
using Gooios.GoodsService.Events;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            IocProvider.GetService<IEventBus>().Subscribe<GoodsShelvedEvent>(new SendGoodsShelvedNotificationHandler());
            IocProvider.GetService<IEventBus>().Subscribe<GoodsSoldOutEvent>(new SendGoodsSoldOutNotificationHandler());
        }
    }
}
