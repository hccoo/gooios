using Gooios.Infrastructure.Events;

namespace Gooios.OrderService.Domains.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
