using Gooios.VerificationService.Domain.Aggregates;
using Gooios.VerificationService.Domain.Repositories;

namespace Gooios.VerificationService.Repositories
{
    public class VerificationRepository : Repository<Verification, int>, IVerificationRepository
    {
        public VerificationRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
