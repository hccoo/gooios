using Gooios.Infrastructure.Events;

namespace Gooios.UserService.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
