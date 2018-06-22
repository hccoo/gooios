using Gooios.Infrastructure.Events;
using Gooios.ImageService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ImageService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<CareermanCreatedEvent>(new SendSmsHandler());
        }
    }
}
