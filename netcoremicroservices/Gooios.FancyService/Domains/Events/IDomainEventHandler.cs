using Gooios.Infrastructure.Events;

namespace Gooios.FancyService.Domains.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
