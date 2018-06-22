using Gooios.Infrastructure.Events;

namespace Gooios.OrganizationService.Domains.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
        
    }
}
