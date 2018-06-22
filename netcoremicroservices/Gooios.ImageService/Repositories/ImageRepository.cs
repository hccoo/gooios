using Gooios.ImageService.Domains.Aggregates;
using Gooios.ImageService.Domains.Repositories;

namespace Gooios.ImageService.Repositories
{
    public class ImageRepository : Repository<Image, string>, IImageRepository
    {
        public ImageRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
