using Gooios.Infrastructure.Events;
using Gooios.UserService.Domain.Aggregates;
using Gooios.UserService.Domain.Repositories;
using Gooios.UserService.Domain;
using Gooios.UserService.Domain.Events;

namespace ZK.SupplyChain.Service.Domain.Events
{
    public class VerificationSentEvent : DomainEvent
    {
        public VerificationSentEvent(IEntity source) : base(source) { }
    }

    public class VerificationSentEventHandler : BaseDomainEventHandler<VerificationSentEvent>, IDomainEventHandler<VerificationSentEvent>
    {
        //readonly IVerificationRepository _verificationRepository;
        private readonly IEventBus _eventBus;

        public VerificationSentEventHandler(IEventBus bus) : base(bus, false)
        {
            //_verificationRepository = verificationRepository;
            _eventBus = bus;
        }

        public override void Do(VerificationSentEvent evnt)
        {
            //var eventSource = evnt.Source as Verification;
            ////eventSource.Status = VerificationStatus.Sent;

            //_verificationRepository.Update(eventSource);
        }
    }
}
