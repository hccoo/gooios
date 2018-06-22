using Gooios.Infrastructure.Events;
using Gooios.VerificationService.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.VerificationService.Domain.Events
{
    public class VerificationCreatedEvent : DomainEvent
    {
        public VerificationCreatedEvent() { }
        public VerificationCreatedEvent(IEntity source) : base(source) { }

        public DateTime CreatedTime { get; set; }
        public string VerificationTo { get; set; }
        public string VerificationCode { get; set; }
        public BizCode BizCode { get; set; }
    }

    public class VerificationCreatedEventHandler : IDomainEventHandler<VerificationCreatedEvent>
    {
        private readonly IEventBus bus;

        public VerificationCreatedEventHandler(IEventBus bus)
        {
            this.bus = bus;
        }
        
        public void Handle(VerificationCreatedEvent evnt)
        {
            //var verification = evnt.Source as Verification;

            //verification.Status = VerificationStatus.Sent;
            //verification.LastUpdOn = DateTime.Now;

            bus.Publish(evnt);
        }
    }
}
