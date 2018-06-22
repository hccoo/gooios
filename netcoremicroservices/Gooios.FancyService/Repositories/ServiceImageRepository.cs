using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class ServiceImageRepository : Repository<ServiceImage, int>, IServiceImageRepository
    {
        public ServiceImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
