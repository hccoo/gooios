using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Repositories;
using Gooios.FancyService.Proxies;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface IServicerAppService : IApplicationServiceContract
    {
        Task AddServicer(ServicerDTO model, string operatorId);

        Task UpdateServicer(ServicerDTO model, string operatorId);

        void SuspendServicer(string id);

        Task<ServicerDTO> GetServicer(string id);

        Task<IEnumerable<ServicerDTO>> GetServicers(string organizationId, int pageIndex, int pageSize);

        Task<IEnumerable<ServicerDTO>> GetNearbyServicers(double longitude, double latitude, int pageIndex, int pageSize, string key, string category, string subCategory, string appId = "GOOIOS001");
    }
    public class ServicerAppService : ApplicationServiceContract, IServicerAppService
    {
        readonly IServicerRepository _servicerRepository;
        readonly IServicerImageRepository _servicerImageRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IImageServiceProxy _imageServiceProxy;
        readonly IOrganizationServiceProxy _organizationServiceProxy;
        readonly IAmapProxy _amapProxy;
        readonly IUserServiceProxy _userProxy;

        public ServicerAppService(IServicerRepository servicerRepository, IUserServiceProxy userProxy, IServicerImageRepository servicerImageRepository, IDbUnitOfWork dbUnitOfWork, IImageServiceProxy imageServiceProxy, IOrganizationServiceProxy organizationServiceProxy, IAmapProxy amapProxy)
        {
            _servicerRepository = servicerRepository;
            _servicerImageRepository = servicerImageRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _imageServiceProxy = imageServiceProxy;
            _organizationServiceProxy = organizationServiceProxy;
            _amapProxy = amapProxy;
            _userProxy = userProxy;
        }

        public async Task AddServicer(ServicerDTO model, string operatorId)
        {
            if (!string.IsNullOrEmpty(model?.Address?.StreetAddress))
            {
                var lonLat = await _amapProxy.Geo(model.Address.StreetAddress);
                model.Address.Longitude = lonLat?.Longitude ?? 0;
                model.Address.Latitude = lonLat?.Latitude ?? 0;
            }

            model.ApplicationId = string.IsNullOrEmpty(model.ApplicationId) ? "GOOIOS001" : model.ApplicationId;
            model.JobNumber = string.IsNullOrEmpty(model.JobNumber) ? Guid.NewGuid().ToString() : model.JobNumber;

            var obj = ServicerFactory.CreateInstance(
                model.JobNumber,
                model.Name,
                model.TechnicalTitle,
                model.TechnicalGrade,
                model.Gender,
                model.BirthDay,
                model.Address,
                model.PersonalIntroduction,
                model.SincerityGoldRate,
                operatorId,
                model.IsSuspend,
                model.OrganizationId, model.Category, model.SubCategory, model.PortraitImageId, model.StartRelevantWorkTime, model.SincerityGold,model.UserName, model.ApplicationId);

            _servicerRepository.Add(obj);

            var ret = await _userProxy.SetServicerIdForUser(model.UserName, obj.Id);

            if (ret)
            {
                if (model.Images != null)
                {
                    foreach (var img in model.Images)
                    {
                        _servicerImageRepository.Add(new ServicerImage
                        {
                            CreatedOn = DateTime.Now,
                            ImageId = img.ImageId,
                            ServicerId = obj.Id
                        });
                    }
                }
                _dbUnitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<ServicerDTO>> GetNearbyServicers(double longitude, double latitude, int pageIndex, int pageSize, string key, string category, string subCategory, string appId = "GOOIOS001")
        {
            var services = new List<ServicerDTO>();
            var skipCount = (pageIndex - 1) * pageSize;
            var result = _servicerRepository.GetPaged(pageIndex, pageSize, 
                o => (o.Name == key || string.IsNullOrEmpty(key)) 
                && (o.Category == category || string.IsNullOrEmpty(category)) 
                && (o.SubCategory == subCategory || string.IsNullOrEmpty(subCategory)) 
                && (string.IsNullOrEmpty(appId) || o.ApplicationId == appId), o => GetDistance(longitude, latitude, o.Longitude, o.Latitude), true);

            foreach (var o in result)
            {
                o.ResolveAddress();

                var servicerImgs = _servicerImageRepository.GetFiltered(g => g.ServicerId == o.Id).ToList();
                var ids = servicerImgs.Select(i => i.ImageId).ToList();
                var imgs = await _imageServiceProxy.GetImagesByIds(ids);

                var organization = await _organizationServiceProxy.GetOrganizationById(o.OrganizationId);
                var logoImgUrl = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                }

                var portrailImageUrl = "";
                var portrailImage = await _imageServiceProxy.GetImageById(o.PortraitImageId);

                if (portrailImage != null)
                {
                    portrailImageUrl = portrailImage.HttpPath;
                }

                services.Add(new ServicerDTO
                {
                    Category = o.Category,
                    Id = o.Id,
                    PersonalIntroduction = o.PersonalIntroduction,
                    OrganizationId = o.OrganizationId,
                    Address = o.Address,
                    BirthDay = o.BirthDay,
                    Gender = o.Gender,
                    IsSuspend = o.IsSuspend,
                    SubCategory = o.SubCategory,
                    JobNumber = o.JobNumber,
                    Images = imgs?.Select(img => new ServicerImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServicerId = o.Id, Title = img.Title }),
                    PortraitImageId = o.PortraitImageId,
                    PortraitImageUrl = portrailImageUrl,
                    Name = o.Name,
                    SincerityGoldRate = o.SincerityGoldRate,
                    StartRelevantWorkTime = o.StartRelevantWorkTime,
                    TechnicalGrade = o.TechnicalGrade,
                    TechnicalTitle = o.TechnicalTitle,
                    SincerityGold = o.SincerityGold,
                    ApplicationId = o.ApplicationId,
                    UserName = o.UserName
                });
            }
            return services;
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

        public async Task<ServicerDTO> GetServicer(string id)
        {
            var servicer = _servicerRepository.Get(id);
            if (servicer == null) return null;

            servicer.ResolveAddress();

            var servicerImgs = _servicerImageRepository.GetFiltered(g => g.ServicerId == servicer.Id).ToList();
            var ids = servicerImgs.Select(i => i.ImageId).ToList();
            var imgs = await _imageServiceProxy.GetImagesByIds(ids);

            var organization = await _organizationServiceProxy.GetOrganizationById(servicer.OrganizationId);
            var logoImgUrl = "";
            if (organization != null)
            {
                var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                logoImgUrl = logoImg?.HttpPath;
            }
            var portrailImageUrl = "";
            var portrailImage = await _imageServiceProxy.GetImageById(servicer.PortraitImageId);

            if (portrailImage != null)
            {
                portrailImageUrl = portrailImage.HttpPath;
            }

            return new ServicerDTO
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
                Images = imgs?.Select(img => new ServicerImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServicerId = servicer.Id, Title = img.Title }),
                PortraitImageId = servicer.PortraitImageId,
                PortraitImageUrl = portrailImageUrl,
                Name = servicer.Name,
                SincerityGoldRate = servicer.SincerityGoldRate,
                StartRelevantWorkTime = servicer.StartRelevantWorkTime,
                TechnicalGrade = servicer.TechnicalGrade,
                TechnicalTitle = servicer.TechnicalTitle,
                SincerityGold = servicer.SincerityGold,
                ApplicationId = servicer.ApplicationId,
                UserName = servicer.UserName
            };
        }

        public async Task<IEnumerable<ServicerDTO>> GetServicers(string organizationId, int pageIndex, int pageSize)
        {
            var servicers = new List<ServicerDTO>();
            var skipCount = (pageIndex - 1) * pageSize;
            var result = _servicerRepository.GetFiltered(o => o.OrganizationId == organizationId).Skip(skipCount).Take(pageSize).ToList();

            foreach (var o in result)
            {
                o.ResolveAddress();

                var serviceImgs = _servicerImageRepository.GetFiltered(g => g.ServicerId == o.Id).ToList();
                var ids = serviceImgs.Select(i => i.ImageId).ToList();
                var imgs = await _imageServiceProxy.GetImagesByIds(ids);

                var organization = await _organizationServiceProxy.GetOrganizationById(o.OrganizationId);
                var logoImgUrl = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                }

                var portrailImageUrl = "";
                var portrailImage = await _imageServiceProxy.GetImageById(o.PortraitImageId);

                if (portrailImage != null)
                {
                    portrailImageUrl = portrailImage.HttpPath;
                }

                servicers.Add(new ServicerDTO
                {
                    Category = o.Category,
                    Id = o.Id,
                    PersonalIntroduction = o.PersonalIntroduction,
                    OrganizationId = o.OrganizationId,
                    Address = o.Address,
                    BirthDay = o.BirthDay,
                    Gender = o.Gender,
                    IsSuspend = o.IsSuspend,
                    SubCategory = o.SubCategory,
                    JobNumber = o.JobNumber,
                    Images = imgs?.Select(img => new ServicerImageDTO { Description = img.Description, HttpPath = img.HttpPath, ImageId = img.Id, ServicerId = o.Id, Title = img.Title }),
                    PortraitImageId = o.PortraitImageId,
                    PortraitImageUrl = portrailImageUrl,
                    Name = o.Name,
                    SincerityGoldRate = o.SincerityGoldRate,
                    StartRelevantWorkTime = o.StartRelevantWorkTime,
                    TechnicalGrade = o.TechnicalGrade,
                    TechnicalTitle = o.TechnicalTitle,
                    SincerityGold = o.SincerityGold,
                    ApplicationId = o.ApplicationId,
                    UserName = o.UserName
                });
            }
            return servicers;
        }

        public void SuspendServicer(string id)
        {
            var obj = _servicerRepository.Get(id);
            if (obj == null) return;

            obj.SetSuspend();

            _servicerRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }

        public async Task UpdateServicer(ServicerDTO model, string operatorId)
        {
            var obj = _servicerRepository.Get(model.Id);
            if (obj == null) return;

            if (!string.IsNullOrEmpty(model?.Address?.StreetAddress))
            {
                var lonLat = await _amapProxy.Geo(model.Address.StreetAddress);
                model.Address.Longitude = lonLat?.Longitude ?? 0;
                model.Address.Latitude = lonLat?.Latitude ?? 0;
            }

            obj.Address = model.Address;
            obj.InitAddress();
            obj.Category = model.Category;
            obj.SubCategory = model.SubCategory;
            obj.LastUpdBy = operatorId;
            obj.LastUpdOn = DateTime.Now;
            obj.BirthDay = model.BirthDay;
            obj.Gender = model.Gender;
            obj.JobNumber = model.JobNumber;
            obj.Name = model.Name;
            obj.PersonalIntroduction = model.PersonalIntroduction;
            obj.PortraitImageId = model.PortraitImageId;
            obj.SincerityGoldRate = model.SincerityGoldRate;
            obj.TechnicalGrade = model.TechnicalGrade;
            obj.TechnicalTitle = model.TechnicalTitle;
            obj.StartRelevantWorkTime = model.StartRelevantWorkTime;
            obj.SincerityGold = model.SincerityGold;
            obj.UserName = model.UserName;

            var imgs = _servicerImageRepository.GetFiltered(o => o.ServicerId == model.Id)?.ToList();

            imgs?.ForEach(item =>
            {
                _servicerImageRepository.Remove(item);
            });

            model.Images?.ToList().ForEach(item =>
            {
                _servicerImageRepository.Add(new ServicerImage { CreatedOn = DateTime.Now, ImageId = item.ImageId, ServicerId = model.Id });
            });

            _servicerRepository.Update(obj);

            var ret = await _userProxy.SetServicerIdForUser(model.UserName, obj.Id);

            if(ret)
                _dbUnitOfWork.Commit();
        }
    }
}
