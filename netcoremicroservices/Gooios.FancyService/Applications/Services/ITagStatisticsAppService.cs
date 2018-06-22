using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Repositories;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface ITagStatisticsAppService : IApplicationServiceContract
    {
        ServiceTagStatisticsDTO GetServiceTagStatistics(string serviceId);

        ServicerTagStatisticsDTO GetServicerTagStatistics(string servicerId);
    }

    public class TagStatisticsAppService : ApplicationServiceContract, ITagStatisticsAppService
    {
        readonly ICommentTagRepository _commentTagRepository;
        readonly ITagRepository _tagRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IServiceRepository _serviceRepository;
        readonly IServicerRepository _servicerRepository;
        readonly IReservationRepository _reservationRepository;

        public TagStatisticsAppService(
            ICommentTagRepository commentTagRepository,
            ITagRepository tagRepository,
            IDbUnitOfWork dbUnitOfWork,
            IServiceRepository serviceRepository,
            IServicerRepository servicerRepository,
            IReservationRepository reservationRepository)
        {
            _tagRepository = tagRepository;
            _commentTagRepository = commentTagRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _serviceRepository = serviceRepository;
            _servicerRepository = servicerRepository;
            _reservationRepository = reservationRepository;
        }

        public ServiceTagStatisticsDTO GetServiceTagStatistics(string serviceId)
        {
            var reservations = _reservationRepository.GetFiltered(o => o.ServiceId == serviceId).ToList();

            var results = new List<TagStatisticsDTO>();

            foreach (var reservation in reservations)
            {
                var ret = _commentTagRepository.GetFiltered(o => o.ReservationId == reservation.Id).GroupBy(g => g.TagId).Select(item => new TagStatisticsDTO { TagId = item.Key, Count = item.Count(), TagName = _tagRepository.Get(item.Key)?.Name }).ToList();
                results.AddRange(ret);
            }

            var result = new List<TagStatisticsDTO>();
            foreach (var item in results)
            {
                var obj = result.FirstOrDefault(o => o.TagId == item.TagId);
                if (obj != null)
                {
                    obj.Count += item.Count;
                }
                else
                {
                    result.Add(item);
                }
            }

            //results.ForEach(item => {
            //    var name = _tagRepository.Get(item.TagId)?.Name;
            //    item.TagName = name;
            //});

            return new ServiceTagStatisticsDTO { ServiceId = serviceId, TagStatistics = result };
        }

        public ServicerTagStatisticsDTO GetServicerTagStatistics(string servicerId)
        {
            var reservations = _reservationRepository.GetFiltered(o => o.ServicerId == servicerId).ToList();

            var results = new List<TagStatisticsDTO>();

            foreach (var reservation in reservations)
            {
                var ret = _commentTagRepository.GetFiltered(o => o.ReservationId == reservation.Id).GroupBy(g => g.TagId).Select(item => new TagStatisticsDTO { TagId = item.Key, Count = item.Count(), TagName = _tagRepository.Get(item.Key)?.Name }).ToList();
                results.AddRange(ret);
            }

            var result = new List<TagStatisticsDTO>();
            foreach (var item in results)
            {
                var obj = result.FirstOrDefault(o => o.TagId == item.TagId);
                if (obj != null)
                {
                    obj.Count += item.Count;
                }
                else
                {
                    result.Add(item);
                }
            }

            //results.ForEach(item => {
            //    var name = _tagRepository.Get(item.TagId)?.Name;
            //    item.TagName = name;
            //});

            return new ServicerTagStatisticsDTO { ServicerId = servicerId, TagStatistics = result };
        }
    }
}
