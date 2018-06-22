using Gooios.FancyService.Domains.Events;
using Gooios.FancyService.Events;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            IocProvider.GetService<IEventBus>().Subscribe<AppointmentConfirmedEvent>(new CreateOrderHandler());
        }
    }
}
