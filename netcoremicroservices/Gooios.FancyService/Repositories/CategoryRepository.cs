using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;

namespace Gooios.FancyService.Domains.Repositories
{
    public class CategoryRepository : Repository<Category, string>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
