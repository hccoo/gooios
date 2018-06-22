using Gooios.Infrastructure.Events;
using Gooios.ActivityService.Domains.Events;

namespace Gooios.ActivityService.Events
{
    [HandlesAsynchronously]
    public class GrouponParticipatedNotificationHandler : IEventHandler<GrouponParticipatedEvent>
    {
        public GrouponParticipatedNotificationHandler()
        {
        }

        public void Handle(GrouponParticipatedEvent evnt)
        {
            // Send sms logic
        }
    }
}
