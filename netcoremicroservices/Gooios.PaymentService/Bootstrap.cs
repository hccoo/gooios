using Gooios.Infrastructure.Events;
using Gooios.PaymentService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<CareermanCreatedEvent>(new SendSmsHandler());
        }
    }
}
