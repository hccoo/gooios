using Gooios.VerificationService.Domain.Aggregates;
using System.Collections.Generic;
using System.Linq;

namespace Gooios.VerificationService.Domain.Services
{
    public interface IVerificationService
    {
        void SetVerificationsSuspend(IEnumerable<Verification> verifications);
    }

    public class VerificationService : IVerificationService
    {
        public void SetVerificationsSuspend(IEnumerable<Verification> verifications) => verifications.ToList().ForEach(item => { item.SetSuspend(); });
    }
}
