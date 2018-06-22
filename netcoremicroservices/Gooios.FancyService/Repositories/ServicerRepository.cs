using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class ServicerRepository : Repository<Servicer, string>, IServicerRepository
    {
        public ServicerRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
