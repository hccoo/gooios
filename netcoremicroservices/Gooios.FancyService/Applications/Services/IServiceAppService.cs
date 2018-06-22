using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Repositories;
using Gooios.FancyService.Proxies;
using Gooios.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface IServiceAppService : IApplicationServiceContract
    {
        Task AddService(ServiceDTO service, string operatorId);

        Task UpdateService(ServiceDTO service, string operatorId);

        void SuspendService(string id);

        void DeleteService(string id);

        Task<ServiceDTO> GetService(string id);

        Task<IEnumerable<ServiceDTO>> GetNearbyServices(double longitude, double latitude, int pageIndex, int pageSize, string key, string category, string subCategory, string appId = "GOOIOS001");

        Task<IEnumerable<ServiceDTO>> GetServices(string organizationId, int pageIndex, int pageSize);
    }
    public class ServiceAppService : ApplicationServiceContract, IServiceAppService
    {
        readonly IServiceRepository _serviceRepository;
        readonly IServiceImageRepository _serviceImageRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IImageServiceProxy _imageServiceProxy;
        readonly IOrganizationServiceProxy _organizationServiceProxy;
        readonly IAmapProxy _amapProxy;
        readonly TmpInstanceGenerate _tmp;

        public ServiceAppService(
            IServiceRepository serviceRepository, 
            IServiceImageRepository serviceImageRepository, 
            IDbUnitOfWork unitOfWork, 
            IImageServiceProxy imageServiceProxy, 
            IOrganizationServiceProxy organizationServiceProxy, 
            IAmapProxy amapProxy,
            TmpInstanceGenerate tmp)
        {
            _tmp = tmp;
            _serviceRepository = serviceRepository;
            _serviceImageRepository = serviceImageRepository;
            _dbUnitOfWork = unitOfWork;
            _imageServiceProxy = imageServiceProxy;
            _organizationServiceProxy = organizationServiceProxy;
            _amapProxy = amapProxy;
        }

        public async Task AddService(ServiceDTO service, string operatorId)
        {
            if (!string.IsNullOrEmpty(service?.Station?.StreetAddress))
            {
                var lonLat = await _amapProxy.Geo(service.Station.StreetAddress);
                service.Station.Longitude = lonLat?.Longitude ?? 0;
                service.Station.Latitude = lonLat?.Latitude ?? 0;
            }

            var obj = ServiceFactory.CreateInstance(
                service.Title,
                service.Introduction,
                service.SincerityGold,
                service.ServeScope,
                service.Category,
                service.SubCategory,
                service.Station,
                service.OrganizationId,
                operatorId,
                service.ApplicationId);

            _serviceRepository.Add(obj);
            if (service.Images != null && service.Images.Count() > 0)
            {
                foreach (var img in service.Images)
                {
                    _serviceImageRepository.Add(new ServiceImage
                    {
                        CreatedOn = DateTime.Now,
                        ImageId = img.ImageId,
                        ServiceId = obj.Id
                    });
                }
            }
            _dbUnitOfWork.Commit();
        }

        public void DeleteService(string id)
        {
            var obj = _serviceRepository.Get(id);
            if (obj == null) return;

            obj.SetDeleted();

            _serviceRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public async Task<IEnumerable<ServiceDTO>> GetNearbyServices(double longitude, double latitude, int pageIndex, int pageSize, string key, string category, string subCategory, string appId = "GOOIOS001")
        {
            var services = new List<ServiceDTO>();
            var skipCount = (pageIndex - 1) * pageSize;

            var result = _serviceRepository.GetPaged(pageIndex, pageSize,
                o => (o.Title.Contains(key) || string.IsNullOrEmpty(key))
                && (o.Category == category || string.IsNullOrEmpty(category))
                && (o.SubCategory == subCategory || string.IsNullOrEmpty(subCategory))
                && (o.Status != ServiceStatus.Deleted)
                && (string.IsNullOrEmpty(appId) || o.ApplicationId == appId)
                && (o.ServeScope >= (GetDistance(longitude, latitude, o.Longitude, o.Latitude)/1000) || o.ServeScope == 0),
                o => GetDistance(longitude, latitude, o.Longitude, o.Latitude), true);

            foreach (var o in result)
            {
                o.ResolveAddress();

                var serviceImgs = _serviceImageRepository.GetFiltered(g => g.ServiceId == o.Id)?.ToList() ?? new List<ServiceImage>();
                var ids = serviceImgs.Select(i => i.ImageId).ToList();
                var imgs = await _imageServiceProxy.GetImagesByIds(ids);
                var servicePhone = "";
                var organization = await _organizationServiceProxy.GetOrganizationById(o.OrganizationId);
                var logoImgUrl = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                    servicePhone = organization.CustomServicePhone;
                }

                services.Add(new ServiceDTO
                {
                    Category = o.Category,
                    Id = o.Id,
                    Introduction = o.Introduction,
                    OrganizationId = o.OrganizationId,
                    ServeScope = o.ServeScope,
                    SincerityGold = o.SincerityGold,
                    Station = o.Station,
                    Status = o.Status,
                    SubCategory = o.SubCategory,
                    Title = o.Title,
                    Images = imgs?.Select(img => new ServiceImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServiceId = o.Id, Title = img.Title }) ?? new List<ServiceImageDTO>(),
                    OrganizationLogoUrl = logoImgUrl,
                    OrganizationName = organization?.ShortName,
                    Distance = ProcessDistance(GetDistance(longitude, latitude, o.Longitude, o.Latitude)),
                    ServicePhone = servicePhone,
                    IsAdvertisement = o.IsAdvertisement,
                    PersonalizedPageUri = o.PersonalizedPageUri,
                    ApplicationId = o.ApplicationId
                });
            }
            return services;
        }

        string ProcessDistance(double distance)
        {
            if (distance < 1000) return $"{String.Format("{0:N0} ", distance)}m";
            return $"{String.Format("{0:N2} ", distance / 1000)}km";
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

        public async Task<ServiceDTO> GetService(string id)
        {
            var service = _serviceRepository.Get(id);
            if (service == null) return null;

            service.ResolveAddress();

            var serviceImgs = _serviceImageRepository.GetFiltered(g => g.ServiceId == id).ToList();
            var ids = serviceImgs.Select(i => i.ImageId).ToList();
            var imgs = await _imageServiceProxy.GetImagesByIds(ids);
            var servicePhone = "";
            var organization = await _organizationServiceProxy.GetOrganizationById(service.OrganizationId);
            var logoImgUrl = "";
            if (organization != null)
            {
                var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                logoImgUrl = logoImg?.HttpPath;
                servicePhone = organization.CustomServicePhone;
            }

            return new ServiceDTO
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
                Images = imgs?.Select(img => new ServiceImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServiceId = service.Id, Title = img.Title }) ?? new List<ServiceImageDTO>(),
                OrganizationLogoUrl = logoImgUrl,
                OrganizationName = organization?.ShortName,
                ServicePhone = servicePhone,
                IsAdvertisement = service.IsAdvertisement,
                PersonalizedPageUri = service.PersonalizedPageUri,
                ApplicationId = service.ApplicationId
            };
        }

        public async Task<IEnumerable<ServiceDTO>> GetServices(string organizationId, int pageIndex, int pageSize)
        {
            var services = new List<ServiceDTO>();
            var skipCount = (pageIndex - 1) * pageSize;
            var result = _serviceRepository.GetFiltered(o => o.OrganizationId == organizationId && (o.Status != ServiceStatus.Deleted)).Skip(skipCount).Take(pageSize).ToList();

            foreach (var o in result)
            {
                o.ResolveAddress();

                var serviceImgs = _serviceImageRepository.GetFiltered(g => g.ServiceId == o.Id).ToList();
                var ids = serviceImgs.Select(i => i.ImageId).ToList();
                var imgs = await _imageServiceProxy.GetImagesByIds(ids);
                var servicePhone = "";
                var organization = await _organizationServiceProxy.GetOrganizationById(o.OrganizationId);
                var logoImgUrl = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                    servicePhone = organization.CustomServicePhone;
                }

                services.Add(new ServiceDTO
                {
                    Category = o.Category,
                    Id = o.Id,
                    Introduction = o.Introduction,
                    OrganizationId = o.OrganizationId,
                    ServeScope = o.ServeScope,
                    SincerityGold = o.SincerityGold,
                    Station = o.Station,
                    Status = o.Status,
                    SubCategory = o.SubCategory,
                    Title = o.Title,
                    Images = imgs?.Select(img => new ServiceImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServiceId = o.Id, Title = img.Title }) ?? new List<ServiceImageDTO>(),
                    OrganizationLogoUrl = logoImgUrl,
                    OrganizationName = organization?.ShortName,
                    ServicePhone = servicePhone,
                    IsAdvertisement = o.IsAdvertisement,
                    PersonalizedPageUri = o.PersonalizedPageUri,
                    ApplicationId = o.ApplicationId
                });
            }
            return services;
        }

        public void SuspendService(string id)
        {
            var obj = _serviceRepository.Get(id);
            if (obj == null) return;

            obj.SetSuspend();

            _serviceRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public async Task UpdateService(ServiceDTO service, string operatorId)
        {
            var obj = _serviceRepository.Get(service.Id);
            if (obj == null) return;

            if (!string.IsNullOrEmpty(service?.Station?.StreetAddress))
            {
                var lonLat = await _amapProxy.Geo(service.Station.StreetAddress);
                service.Station.Longitude = lonLat?.Longitude ?? 0;
                service.Station.Latitude = lonLat?.Latitude ?? 0;
            }

            obj.Station = service.Station;
            obj.InitAddress();
            obj.Category = service.Category;
            obj.Introduction = service.Introduction;
            obj.LastUpdBy = operatorId;
            obj.LastUpdOn = DateTime.Now;
            obj.ServeScope = service.ServeScope;
            obj.SincerityGold = service.SincerityGold;
            obj.SubCategory = service.SubCategory;
            obj.Title = service.Title;

            _serviceRepository.Update(obj);

            var imgs = _serviceImageRepository.GetFiltered(o => o.ServiceId == service.Id)?.ToList();

            imgs?.ForEach(item =>
            {
                _serviceImageRepository.Remove(item);
            });

            service.Images?.ToList().ForEach(item =>
            {
                _serviceImageRepository.Add(new ServiceImage { CreatedOn = DateTime.Now, ImageId = item.ImageId, ServiceId = service.Id });
            });

            _dbUnitOfWork.Commit();
        }
    }



    public class TmpInstanceGenerate
    {
        readonly IDbUnitOfWork _uow;
        readonly IDbUnitOfWork _uow2;
        public TmpInstanceGenerate(IDbUnitOfWork uow, IDbUnitOfWork uow2)
        {
            _uow = uow;
            _uow2 = uow2;
        }

        public void InstanceCall()
        {
            var uow1 = IocProvider.GetService<IDbUnitOfWork>();
            Console.WriteLine("InstanceCall uow2 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow1.GetHashCode());

            using (var scope = IocProvider.Container.CreateScope())
            {
                var uow2 = scope.ServiceProvider.GetService<IDbUnitOfWork>();//IocProvider.GetService<IDbUnitOfWork>();
                Console.WriteLine("InstanceCall uow5 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow2.GetHashCode());
                var uow3 = scope.ServiceProvider.GetService<IDbUnitOfWork>();
                Console.WriteLine("InstanceCall uow6 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow3.GetHashCode());
            }
            var uow4 = IocProvider.GetService<IDbUnitOfWork>();
            Console.WriteLine("InstanceCall uow4 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow4.GetHashCode());
        }
    }
}
