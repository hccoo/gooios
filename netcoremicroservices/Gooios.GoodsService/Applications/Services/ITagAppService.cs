using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.Services
{
    public interface ITagAppService : IApplicationServiceContract
    {
        IEnumerable<TagDTO> GetByGoodsId(string goodsId);
    }
    public class TagAppService : ApplicationServiceContract, ITagAppService
    {
        readonly ITagRepository _tagRepo;
        public TagAppService(ITagRepository tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public IEnumerable<TagDTO> GetByGoodsId(string goodsId)
        {
            var results = _tagRepo.GetByGoodsId(goodsId);

            return results.Select(item =>
            {
                return new TagDTO
                {
                    CategoryId = item.CategoryId,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    Name = item.Name,
                    Id = item.Id
                };
            }).ToList();

        }
    }
}
