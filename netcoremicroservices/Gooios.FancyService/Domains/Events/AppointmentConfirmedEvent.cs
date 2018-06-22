using Gooios.FancyService.Domains.Aggregates;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Events
{
    public class AppointmentConfirmedEvent : DomainEvent
    {
        public AppointmentConfirmedEvent() { }
        public AppointmentConfirmedEvent(IEntity source) : base(source) { }

        public DateTime ConfirmedTime { get; set; }
    }
    public class AppointmentConfirmedEventHandler : IDomainEventHandler<AppointmentConfirmedEvent>
    {
        private readonly IEventBus _bus;

        public AppointmentConfirmedEventHandler(IEventBus bus)
        {
            _bus = bus;
        }
        public void Handle(AppointmentConfirmedEvent evnt)
        {
            var eventSource = evnt.Source as Reservation;
          
            _bus.Publish(evnt);
        }
    }
}
