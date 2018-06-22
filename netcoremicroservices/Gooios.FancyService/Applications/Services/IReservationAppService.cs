using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Repositories;
using Gooios.FancyService.Proxies;
using Gooios.FancyService.Proxies.DTOs;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface IReservationAppService : IApplicationServiceContract
    {
        Task<ReservationDTO> AddReservation(ReservationDTO model, string operatorId);

        void SetReservationUnderway(string id);

        void SetReservationCompleted(string id);

        void SetReservationFailed(string id);

        void SetReservationCancelled(string id);

        void SetOrderId(ReservationDTO model);

        Task<ReservationDTO> GetReservation(string id);

        IEnumerable<ReservationDTO> GetByUserId(string userId, int pageIndex, int pageSize);

        IEnumerable<ReservationDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize);
    }

    public class ReservationAppService : ApplicationServiceContract, IReservationAppService
    {
        readonly IReservationRepository _reservationRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly IServiceRepository _serviceRepository;
        readonly IServicerRepository _servicerRepository;
        readonly IOrganizationServiceProxy _organizationServiceProxy;
        readonly IAmapProxy _amapProxy;
        readonly IOrderServiceProxy _orderServiceProxy;

        public ReservationAppService(IReservationRepository reservationRepository,
            IDbUnitOfWork dbUnitOfWork,
            IEventBus eventBus,
            IServiceRepository serviceRepository,
            IServicerRepository servicerRepository,
            IOrganizationServiceProxy organizationServiceProxy,
            IAmapProxy amapProxy,
            IOrderServiceProxy orderServiceProxy)
        {
            _reservationRepository = reservationRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _serviceRepository = serviceRepository;
            _servicerRepository = servicerRepository;
            _organizationServiceProxy = organizationServiceProxy;
            _amapProxy = amapProxy;
            _orderServiceProxy = orderServiceProxy;
        }

        public async Task<ReservationDTO> AddReservation(ReservationDTO model, string operatorId)
        {
            ReservationDTO result = null;

            var service = _serviceRepository.Get(model.ServiceId);
            var servicer = _servicerRepository.Get(model.ServicerId);
            if (service == null && servicer == null) throw new Exception("异常预约!");

            if (service.ServeScope > 0)
            {
                var res = await _amapProxy.Geo(model?.ServiceDestination?.StreetAddress);
                if (res == null) throw new Exception("地址异常！");

                var sres = await _amapProxy.Geo(service.StreetAddress);
                if (sres != null)
                {
                    var distance = GetDistance(res.Longitude, res.Latitude, sres.Longitude, sres.Latitude);
                    if (distance > service.ServeScope)
                    {
                        throw new Exception("超出范围!");
                    }
                }
            }

            var reservation = ReservationFactory.CreateInstance(
                service,
                servicer,
                model.ServiceDestination,
                model.CustomerName,
                model.CustomerMobile,
                model.AppointTime,
                service.SincerityGold,
                operatorId);

            _reservationRepository.Add(reservation);            
            _dbUnitOfWork.Commit();

            #region create order
            var orderItems = new List<OrderItemDTO>();

            orderItems.Add(new OrderItemDTO
            {
                Count = 1,
                ObjectId = reservation.Id,
                ObjectNo = reservation.ReservationNo,
                Title = reservation.Service?.Title ?? "",
                TradeUnitPrice = reservation.SincerityGoldNeedToPay,
                SelectedProperties = string.Empty,
                PreviewPictureUrl = string.Empty
            });

            await _orderServiceProxy.CreateOrder(new Proxies.DTOs.OrderDTO
            {
                CreatedBy = reservation.CreatedBy,
                CustomerAddress = reservation.ServiceDestination,
                CustomerMobile = reservation.CustomerMobile,
                CustomerName = reservation.CustomerName,
                InvoiceType = InvoiceType.None,
                PayAmount = orderItems.Sum(o => o.TradeUnitPrice * o.Count),
                PreferentialAmount = 0,
                ShippingCost = 0,
                Tax = 0,
                TotalAmount = reservation.SincerityGoldNeedToPay,
                OrderItems = orderItems,
                Mark = "Reservation",
                Invoiceremark = "",
                Remark = $"预约时间：{reservation?.AppointTime},预约服务：{reservation?.Service?.Title},服务者：{reservation?.Servicer?.Name ?? "未指定"}",
                OrganizationId = reservation.OrganizationId
            });
            #endregion 

            reservation.ConfirmAppoint();
            _eventBus.Commit();

            return new ReservationDTO
            {
                AppointTime = reservation.AppointTime,
                CustomerMobile = reservation.CustomerMobile,
                CustomerName = reservation.CustomerName,
                Id = reservation.Id,
                ReservationNo = reservation.ReservationNo,
                ServiceDestination = reservation.ServiceDestination,
                ServiceId = reservation.ServiceId,
                ServicerId = reservation.ServicerId,
                SincerityGoldNeedToPay = reservation.SincerityGoldNeedToPay,
                Status = reservation.Status,
                OrganizationId = reservation.OrganizationId,
                OrderId = reservation.OrderId
            };

        }

        public async Task<ReservationDTO> GetReservation(string id)
        {
            var obj = _reservationRepository.Get(id);
            if (obj == null) return null;
            var result = new ReservationDTO
            {
                AppointTime = obj.AppointTime,
                CustomerMobile = obj.CustomerMobile,
                CustomerName = obj.CustomerName,
                Id = obj.Id,
                ReservationNo = obj.ReservationNo,
                ServiceDestination = obj.ServiceDestination,
                ServiceId = obj.ServiceId ?? "",
                ServicerId = obj.ServicerId ?? "",
                SincerityGoldNeedToPay = obj.SincerityGoldNeedToPay,
                Status = obj.Status,
                OrganizationId = obj.OrganizationId,
                OrderId = obj.OrderId
            };

            if (!string.IsNullOrEmpty(obj.ServiceId))
            {
                var service = _serviceRepository.Get(obj.ServiceId);

                if (service != null)
                {
                    service.ResolveAddress();

                    var organization = await _organizationServiceProxy.GetOrganizationById(service.OrganizationId);

                    var serviceDTO = new ServiceDTO
                    {
                        Category = service.Category,
                        Id = service.Id,
                        Introduction = service.Introduction,
                        OrganizationId = service.OrganizationId,
                        ServeScope = service.ServeScope,
                        SincerityGold = service.SincerityGold,
                        Station = service.Station,
                        Status = service.Status,
                        SubCategory = service.SubCategory,
                        Title = service.Title,
                        OrganizationName = organization?.ShortName
                    };
                    result.Service = serviceDTO;
                }
            }
            if (!string.IsNullOrEmpty(obj.ServicerId))
            {
                var servicer = _servicerRepository.Get(obj.ServicerId);
                if (servicer != null)
                {
                    servicer.ResolveAddress();
                    var servicerDTO = new ServicerDTO
                    {
                        Category = servicer.Category,
                        Id = servicer.Id,
                        PersonalIntroduction = servicer.PersonalIntroduction,
                        OrganizationId = servicer.OrganizationId,
                        Address = servicer.Address,
                        BirthDay = servicer.BirthDay,
                        Gender = servicer.Gender,
                        IsSuspend = servicer.IsSuspend,
                        SubCategory = servicer.SubCategory,
                        JobNumber = servicer.JobNumber,
                        PortraitImageId = servicer.PortraitImageId,
                        Name = servicer.Name,
                        SincerityGoldRate = servicer.SincerityGoldRate,
                        StartRelevantWorkTime = servicer.StartRelevantWorkTime,
                        TechnicalGrade = servicer.TechnicalGrade,
                        TechnicalTitle = servicer.TechnicalTitle
                    };
                    result.Servicer = servicerDTO;
                }
            }

            return result;
        }

        public void SetOrderId(ReservationDTO model)
        {
            var obj = _reservationRepository.Get(model.Id);
            if (obj == null) return;

            obj.OrderId = model.OrderId;
            _reservationRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public void SetReservationCancelled(string id)
        {
            var obj = _reservationRepository.Get(id);
            if (obj == null) return;

            obj.SetCancelledStatus();
            _reservationRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public void SetReservationCompleted(string id)
        {
            var obj = _reservationRepository.Get(id);
            if (obj == null) return;

            obj.SetCompletedStatus();
            _reservationRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public void SetReservationFailed(string id)
        {
            var obj = _reservationRepository.Get(id);
            if (obj == null) return;

            obj.SetFailedStatus();
            _reservationRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public void SetReservationUnderway(string id)
        {
            var obj = _reservationRepository.Get(id);
            if (obj == null) return;

            obj.SetUnderwayStatus();
            _reservationRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public IEnumerable<ReservationDTO> GetByUserId(string userId, int pageIndex, int pageSize)
        {
            return _reservationRepository.GetPaged(pageIndex, pageSize, o => o.CreatedBy == userId, o => o.CreatedOn, false).Select(item =>
            {
                return new ReservationDTO
                {
                    AppointTime = item.AppointTime,
                    CustomerMobile = item.CustomerMobile,
                    CustomerName = item.CustomerName,
                    Id = item.Id,
                    ReservationNo = item.ReservationNo,
                    ServiceDestination = item.ServiceDestination,
                    ServiceId = item.ServiceId,
                    ServicerId = item.ServicerId,
                    SincerityGoldNeedToPay = item.SincerityGoldNeedToPay,
                    Status = item.Status,
                    OrganizationId = item.OrganizationId,
                    OrderId = item.OrderId
                };
            }).ToList();
        }

        public IEnumerable<ReservationDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize)
        {
            return _reservationRepository.GetPaged(pageIndex, pageSize, o => o.OrganizationId == organizationId, o => o.CreatedOn, false).Select(item =>
            {
                return new ReservationDTO
                {
                    AppointTime = item.AppointTime,
                    CustomerMobile = item.CustomerMobile,
                    CustomerName = item.CustomerName,
                    Id = item.Id,
                    ReservationNo = item.ReservationNo,
                    ServiceDestination = item.ServiceDestination,
                    ServiceId = item.ServiceId,
                    ServicerId = item.ServicerId,
                    SincerityGoldNeedToPay = item.SincerityGoldNeedToPay,
                    Status = item.Status,
                    OrganizationId = item.OrganizationId,
                    OrderId = item.OrderId
                };
            }).ToList();
        }

        #region 根据经纬度计算距离
        const double EARTH_RADIUS = 6378137;
        double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }
        double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
        #endregion
    }
}
