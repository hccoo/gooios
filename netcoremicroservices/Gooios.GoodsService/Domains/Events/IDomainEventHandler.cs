using Gooios.Infrastructure.Events;

namespace Gooios.GoodsService.Domains.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
