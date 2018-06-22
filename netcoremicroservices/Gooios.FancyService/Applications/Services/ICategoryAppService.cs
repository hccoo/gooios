using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Repositories;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gooios.FancyService.Applications.Services
{
    public interface ICategoryAppService : IApplicationServiceContract
    {
        void AddServiceCategory(CategoryDTO categoryDTO, string operatorId);

        IEnumerable<CategoryDTO> GetCategoriesByMark(string mark);

        IEnumerable<CategoryDTO> GetCategoriesByParentId(string id);
    }
    public class CategoryAppService : ApplicationServiceContract, ICategoryAppService
    {
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly ICategoryRepository _serviceCategoryRepository;
        readonly ITagRepository _tagRepository;

        public CategoryAppService(IDbUnitOfWork dbUnitOfWork, IEventBus eventBus, ICategoryRepository serviceCategoryRepository, ITagRepository tagRepository)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _serviceCategoryRepository = serviceCategoryRepository;
            _tagRepository = tagRepository;
        }

        public void AddServiceCategory(CategoryDTO categoryDTO, string operatorId)
        {
            var obj = new Domains.Aggregates.Category
            {
                Name = categoryDTO.Name,
                ParentId = categoryDTO.ParentId,
                Mark = categoryDTO.Mark,
                ApplicationId = categoryDTO.AppId
            };

            obj.GenerateId();
            _serviceCategoryRepository.Add(obj);

            if (categoryDTO.Tags != null && categoryDTO.Tags.Count() > 0)
            {
                foreach (var item in categoryDTO.Tags)
                {
                    var o = new Domains.Aggregates.Tag { CategoryId = obj.Id, CreatedBy = operatorId, CreatedOn = DateTime.Now, Name = item.Name };
                    o.GenerateId();
                    _tagRepository.Add(o);
                }
            }

            _dbUnitOfWork.Commit();
        }

        public IEnumerable<CategoryDTO> GetCategoriesByMark(string mark)
        {
            var categories = _serviceCategoryRepository.GetFiltered(o => o.Mark == mark).OrderBy(g=>g.Order);
            return categories.Select(item => new CategoryDTO { Id = item.Id, Name = item.Name, ParentId = item.ParentId, Mark = item.Mark, AppId=item.ApplicationId, Order=item.Order }).ToList();
        }

        public IEnumerable<CategoryDTO> GetCategoriesByParentId(string id)
        {
            var categories = _serviceCategoryRepository.GetFiltered(o => o.ParentId == id).OrderBy(g => g.Order);
            return categories.Select(item => new CategoryDTO { Id = item.Id, Name = item.Name, ParentId = item.ParentId, Mark = item.Mark, AppId = item.ApplicationId,Order = item.Order }).ToList();
        }
    }
}
