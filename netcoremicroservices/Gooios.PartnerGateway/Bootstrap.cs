using Gooios.Infrastructure.Events;
using Gooios.PartnerGateway.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PartnerGateway
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<CareermanCreatedEvent>(new SendSmsHandler());
        }
    }
}
