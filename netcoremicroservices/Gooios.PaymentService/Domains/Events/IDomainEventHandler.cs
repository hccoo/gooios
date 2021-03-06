﻿using Gooios.Infrastructure.Events;

namespace Gooios.PaymentService.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }
}
