using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Repositories;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface ITagAppService : IApplicationServiceContract
    {
        IEnumerable<TagDTO> GetByReservationId(string reservationId);
    }

    public class TagAppService : ApplicationServiceContract, ITagAppService
    {
        readonly ITagRepository _tagRepo;
        public TagAppService(ITagRepository tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public IEnumerable<TagDTO> GetByReservationId(string reservationId)
        {
            var results = _tagRepo.GetByReservationId(reservationId);

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
