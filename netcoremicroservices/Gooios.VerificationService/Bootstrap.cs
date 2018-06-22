using Gooios.Infrastructure.Events;
using Gooios.VerificationService.Domain.Events;
using Gooios.VerificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.VerificationService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            IocProvider.GetService<IEventBus>().Subscribe<VerificationCreatedEvent>(new SendSmsHandler());
        }
    }
}
