using Gooios.VerificationService.Domain.Aggregates;
using Gooios.VerificationService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.VerificationService.Domain.Events
{
    public class VerificationSentEvent : DomainEvent
    {
        public VerificationSentEvent(IEntity source) : base(source) { }
    }

    public class VerificationSentEventHandler : IDomainEventHandler<VerificationSentEvent>
    {
        readonly IVerificationRepository _verificationRepository;

        public VerificationSentEventHandler(IVerificationRepository verificationRepository)
        {
            verificationRepository = _verificationRepository;
        }

        public void Handle(VerificationSentEvent evnt)
        {
            var eventSource = evnt.Source as Verification;
            //eventSource.Status = VerificationStatus.Sent;

            _verificationRepository.Update(eventSource);
        }
    }
}
