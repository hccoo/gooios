using Gooios.Infrastructure.Events;
using Gooios.PaymentGateway.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentGateway
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<CareermanCreatedEvent>(new SendSmsHandler());
        }
    }
}
