using Gooios.Infrastructure;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Applications.Services
{
    public interface IDeliveryAddressAppService : IApplicationServiceContract
    {
        void AddDeliveryAddress(DeliveryAddressDTO model);

        IEnumerable<DeliveryAddressDTO> GetMyDeliveryAddresses(string userId);
    }

    public class DeliveryAddressAppService : ApplicationServiceContract, IDeliveryAddressAppService
    {
        readonly IDeliveryAddressRepository _deliveryAddressRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        public DeliveryAddressAppService(IDeliveryAddressRepository deliveryAddressRepository, IDbUnitOfWork dbUnitOfWork)
        {
            _deliveryAddressRepository = deliveryAddressRepository;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void AddDeliveryAddress(DeliveryAddressDTO model)
        {
            var da = DeliveryAddressFactory.CreateInstance(model.UserId, model.LinkMan, model.Gender, model.Mobile, model.Mark, model.IsDefault, model.Province, model.City, model.Area, model.StreetAddress, model.Postcode, model.CreatedBy);
            _deliveryAddressRepository.Add(da);
            _dbUnitOfWork.Commit();
        }

        public IEnumerable<DeliveryAddressDTO> GetMyDeliveryAddresses(string userId)
        {
            var objs = _deliveryAddressRepository.GetFiltered(o => o.UserId == userId).Select(item => new DeliveryAddressDTO
            {
                Id = item.Id,
                Area = item.Area,
                CreatedBy = item.CreatedBy,
                City = item.City,
                UserId = item.UserId,
                CreatedOn = item.CreatedOn,
                Gender = item.Gender,
                IsDefault = item.IsDefault,
                LinkMan = item.LinkMan,
                Mark = item.Mark,
                Mobile = item.Mobile,
                Postcode = item.Postcode,
                Province = item.Province,
                StreetAddress = item.StreetAddress
            }).ToList();
            return objs;
        }
    }
}
