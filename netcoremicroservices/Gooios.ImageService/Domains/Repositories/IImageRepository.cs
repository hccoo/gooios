using Gooios.ImageService.Domain;
using Gooios.ImageService.Domains.Aggregates;

namespace Gooios.ImageService.Domains.Repositories
{
    public interface IImageRepository : IRepository<Image, string>
    {
    }
}
