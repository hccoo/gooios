using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AppletUserService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<VerificationCreatedEvent>(new SendSmsHandler());
        }
    }
}
