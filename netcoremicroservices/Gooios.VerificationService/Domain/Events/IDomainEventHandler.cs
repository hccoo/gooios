using Gooios.Infrastructure.Events;

namespace Gooios.VerificationService.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
