using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Gooios.Infrastructure.Events;
using Gooios.UserService.Domain;
using Gooios.UserService.Domain.Aggregates;
using Gooios.UserService.Domain.Events;

namespace Gooios.UserService.Events
{
    [HandlesAsynchronously]
    public class SendSmsHandler : INotificationHandler<VerificationCreatedEvent> //IEventHandler<VerificationCreatedEvent>
    {
        public SendSmsHandler()
        {
        }

        public Task Handle(VerificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
