using Gooios.ActivityService.Domains.Events;
using Gooios.ActivityService.Events;
using Gooios.Infrastructure.Events;

namespace Gooios.ActivityService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            IocProvider.GetService<IEventBus>().Subscribe<GrouponParticipatedEvent>(new GrouponParticipatedNotificationHandler());
        }
    }
}
