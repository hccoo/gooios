using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class ServicerImageRepository : Repository<ServicerImage, int>, IServicerImageRepository
    {
        public ServicerImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
