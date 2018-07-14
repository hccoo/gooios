using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.Services
{
    public interface IGoodsCategoryAppService : IApplicationServiceContract
    {
        void AddGoodsCategory(GoodsCategoryDTO goodsCategoryDTO, string operatorId);

        IEnumerable<GoodsCategoryDTO> GetAllGoodsCategories(string appId = "");
    }

    public class GoodsCategoryAppService : ApplicationServiceContract, IGoodsCategoryAppService
    {
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly IGoodsCategoryRepository _goodsCategoryRepository;
        readonly ITagRepository _tagRepository;

        public GoodsCategoryAppService(IDbUnitOfWork dbUnitOfWork, IEventBus eventBus, IGoodsCategoryRepository goodsCategoryRepository, ITagRepository tagRepository)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _goodsCategoryRepository = goodsCategoryRepository;
            _tagRepository = tagRepository;
        }

        public void AddGoodsCategory(GoodsCategoryDTO goodsCategoryDTO, string operatorId)
        {
            var obj = new Domains.Aggregates.GoodsCategory
            {
                Name = goodsCategoryDTO.Name,
                ParentId = goodsCategoryDTO.ParentId
            };

            obj.GenerateId();
            _goodsCategoryRepository.Add(obj);

            if (goodsCategoryDTO.Tags != null && goodsCategoryDTO.Tags.Count() > 0)
            {
                foreach (var item in goodsCategoryDTO.Tags)
                {
                    var o = new Domains.Aggregates.Tag { CategoryId = obj.Id, CreatedBy = operatorId, CreatedOn = DateTime.Now, Name = item.Name };
                    o.GenerateId();
                    _tagRepository.Add(o);
                }
            }

            _dbUnitOfWork.Commit();
        }

        public IEnumerable<GoodsCategoryDTO> GetAllGoodsCategories(string appId = "")
        {
            IEnumerable<GoodsCategoryDTO> result;

            if(!string.IsNullOrEmpty(appId))
                result = _goodsCategoryRepository.GetFiltered(o => o.ApplicationId == appId).Select(item => new GoodsCategoryDTO { Id = item.Id, Name = item.Name, ParentId = item.ParentId, Order = item.Order }).OrderBy(o => o.Order).ToList();
            else
                result = _goodsCategoryRepository.GetAll().Select(item => new GoodsCategoryDTO { Id = item.Id, Name = item.Name, ParentId = item.ParentId, Order = item.Order }).OrderBy(o => o.Order).ToList();
            
            return result;
        }
    }
}
