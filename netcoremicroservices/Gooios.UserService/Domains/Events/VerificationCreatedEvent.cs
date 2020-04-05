using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Gooios.UserService.Domain.Aggregates;
using System.Threading;

namespace Gooios.UserService.Domain.Events
{
    public class VerificationCreatedEvent : DomainEvent, INotification
    {
        public VerificationCreatedEvent() { }
        public VerificationCreatedEvent(IEntity source) : base(source) { }

        public DateTime CreatedTime { get; set; }
        public string VerificationTo { get; set; }
        public string VerificationCode { get; set; }
        //public BizCode BizCode { get; set; }
    }

    //[HandlesAsynchronously]
    public class VerificationCreatedEventHandler : BaseDomainEventHandler<VerificationCreatedEvent>, IDomainEventHandler<VerificationCreatedEvent>
    {
        private readonly IEventBus bus;

        public VerificationCreatedEventHandler(IEventBus bus):base(bus,true)
        {
            this.bus = bus;
        }
        
        public override void Do(VerificationCreatedEvent evnt)
        {
            //var verification = evnt.Source as Verification;

            //verification.Status = VerificationStatus.Sent;
            //verification.LastUpdOn = DateTime.Now;

            //bus.Publish(evnt);
        }
    }
}
