using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class ServiceRepository : Repository<Service, string>, IServiceRepository
    {
        public ServiceRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
