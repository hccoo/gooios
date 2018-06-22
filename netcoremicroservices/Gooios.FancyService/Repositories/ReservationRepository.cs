using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class ReservationRepository : Repository<Reservation, string>, IReservationRepository
    {
        public ReservationRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
