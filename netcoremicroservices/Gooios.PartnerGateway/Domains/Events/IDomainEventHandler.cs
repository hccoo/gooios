using Gooios.Infrastructure.Events;

namespace Gooios.PartnerGateway.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
