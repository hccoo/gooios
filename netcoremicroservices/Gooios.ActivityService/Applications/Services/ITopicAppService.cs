using Gooios.ActivityService.Applications.DTOs;
using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Domains.Repositories;
using Gooios.ActivityService.Proxies;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Applications.Services
{
    public interface ITopicAppService : IApplicationServiceContract
    {
        Task<TopicDTO> AddTopic(TopicDTO model, string operatorId);

        Task UpdateTopic(TopicDTO model, string operatorId);

        Task<IEnumerable<TopicDTO>> GetTopicsByOrganizationId(int pageIndex, int pageSize, string organizationId, string key = "");

        Task<IEnumerable<TopicDTO>> GetNearbyTopics(double longitude, double latitude, int pageIndex, int pageSize, string key = "", string appId = "");

        Task<TopicDTO> GetById(string topicId, double? longitude = null, double? latitude = null);
    }

    public class TopicAppService : ApplicationServiceContract, ITopicAppService
    {
        readonly ITopicRepository _topicRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IOrganizationServiceProxy _organizationServiceProxy;
        readonly IAmapProxy _amapProxy;
        readonly ITopicImageRepository _topicImageRepository;
        readonly IImageServiceProxy _imageServiceProxy;

        public TopicAppService(ITopicRepository topicRepository,
            IDbUnitOfWork dbUnitOfWork,
            IOrganizationServiceProxy organizationServiceProxy,
            IAmapProxy amapProxy,
            ITopicImageRepository topicImageRepository,
            IImageServiceProxy imageServiceProxy)
        {
            _topicRepository = topicRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _organizationServiceProxy = organizationServiceProxy;
            _amapProxy = amapProxy;
            _topicImageRepository = topicImageRepository;
            _imageServiceProxy = imageServiceProxy;
        }

        public async Task<TopicDTO> AddTopic(TopicDTO model, string operatorId)
        {
            var now = DateTime.Now;
            var count = _topicRepository.GetFiltered(o => o.OrganizationId == model.OrganizationId && (o.EndDate == null || o.EndDate >= now)).Count();

            if (count >= 3) throw new Exception("商户最多发布3条主题。");

            if (!string.IsNullOrEmpty(model?.Address?.StreetAddress))
            {
                var lonLat = await _amapProxy.Geo(model.Address.StreetAddress);
                model.Address.Longitude = lonLat?.Longitude ?? 0;
                model.Address.Latitude = lonLat?.Latitude ?? 0;
            }

            var obj = TopicFactory.CreateInstance(
                 model.Title, model.FaceImageUrl, model.Introduction, model.IsCustom, model.CustomTopicUrl, model.OrganizationId, model.Address.Province,
                 model.Address.City, model.Address.Area, model.Address.StreetAddress, model.Address.Postcode, model.Address.Latitude, model.Address.Longitude,
                 operatorId, model.CreatorName, model.CreatorPortraitUrl, model.StartDate, model.EndDate, model.Category
                );

            _topicRepository.Add(obj);

            if (model.ContentImages != null && model.ContentImages.Count() > 0)
            {
                var i = 0;
                foreach (var img in model.ContentImages)
                {
                    i++;
                    _topicImageRepository.Add(new TopicImage
                    {
                        ImageId = img.ImageId,
                        ImageUrl = img.ImageUrl,
                        TopicId = obj.Id,
                        Order = i
                    });
                }
            }
            _dbUnitOfWork.Commit();

            return new TopicDTO
            {
                Address = obj.Address,
                Category = obj.Category,
                ContentImages = model.ContentImages.Select(item => new TopicImageDTO { ImageId = item.ImageId, ImageUrl = item.ImageUrl, TopicId = obj.Id }).ToList(),
                CreatedBy = obj.CreatedBy,
                CreatedOn = obj.CreatedOn,
                CustomTopicUrl = obj.CustomTopicUrl,
                EndDate = obj.EndDate,
                FaceImageUrl = obj.FaceImageUrl,
                Introduction = obj.Introduction,
                IsCustom = obj.IsCustom,
                IsSuspend = obj.IsSuspend,
                LastUpdBy = obj.LastUpdBy,
                LastUpdOn = obj.LastUpdOn,
                OrganizationId = obj.OrganizationId,
                StartDate = obj.StartDate,
                Title = obj.Title,
                Id = obj.Id,
                CreatorName = obj.CreatorName,
                CreatorPortraitUrl = obj.CreatorPortraitUrl
            };
        }

        public async Task<TopicDTO> GetById(string topicId, double? longitude = null, double? latitude = null)
        {
            var result = _topicRepository.Get(topicId);
            result.ResolveAddress();
            var imgs = _topicImageRepository.GetFiltered(o => o.TopicId == topicId).OrderBy(g => g.Order).Select(img => new TopicImageDTO
            {
                Id = img.Id,
                ImageId = img.ImageId,
                ImageUrl = img.ImageUrl,
                TopicId = img.TopicId,
                Order = img.Order
            });

            var org = await _organizationServiceProxy.GetOrganizationById(result.OrganizationId);

            string distance = "很遥远";
            if (longitude != null && latitude != null)
            {
                var dis = GetDistance(result.Longitude, result.Latitude, longitude.Value, latitude.Value);
                distance = ProcessDistance(dis);
            }

            return new TopicDTO
            {
                Address = result.Address,
                Category = result.Category,
                ContentImages = imgs,
                CreatedBy = result.CreatedBy,
                CreatedOn = result.CreatedOn,
                CustomTopicUrl = result.CustomTopicUrl,
                EndDate = result.EndDate,
                FaceImageUrl = result.FaceImageUrl,
                Id = result.Id,
                Introduction = result.Introduction,
                IsCustom = result.IsCustom,
                IsSuspend = result.IsSuspend,
                LastUpdBy = result.LastUpdBy,
                LastUpdOn = result.LastUpdOn,
                OrganizationId = result.OrganizationId,
                OrganizationName = org?.ShortName,
                StartDate = result.StartDate,
                Title = result.Title,
                Distance = distance,
                CreatorName = result.CreatorName,
                CreatorPortraitUrl = result.CreatorPortraitUrl
            };

        }

        public async Task<IEnumerable<TopicDTO>> GetNearbyTopics(double longitude, double latitude, int pageIndex, int pageSize, string key = "", string appId = "")
        {
            var topics = new List<TopicDTO>();
            var skipCount = (pageIndex - 1) * pageSize;

            var result = _topicRepository.GetPaged(pageIndex, pageSize,
                o => (o.Title.Contains(key) || string.IsNullOrEmpty(key)) && (o.ApplicationId == appId || string.IsNullOrEmpty(appId)) && (o.IsSuspend == false),
                o => GetDistance(longitude, latitude, o.Longitude, o.Latitude), true);

            foreach (var o in result)
            {
                o.ResolveAddress();

                var serviceImgs = _topicImageRepository.GetFiltered(g => g.TopicId == o.Id)?.ToList() ?? new List<TopicImage>();
                var servicePhone = "";
                var organization = await _organizationServiceProxy.GetOrganizationById(o.OrganizationId);
                var logoImgUrl = "";
                var shortName = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                    servicePhone = organization.CustomServicePhone;
                    shortName = organization.ShortName;
                }

                topics.Add(new TopicDTO
                {
                    Address = o.Address,
                    Category = o.Category,
                    //ContentImages = imgs,
                    CreatedBy = o.CreatedBy,
                    CreatedOn = o.CreatedOn,
                    CustomTopicUrl = o.CustomTopicUrl,
                    EndDate = o.EndDate,
                    FaceImageUrl = o.FaceImageUrl,
                    Id = o.Id,
                    Introduction = o.Introduction,
                    IsCustom = o.IsCustom,
                    IsSuspend = o.IsSuspend,
                    LastUpdBy = o.LastUpdBy,
                    LastUpdOn = o.LastUpdOn,
                    OrganizationId = o.OrganizationId,
                    OrganizationName = shortName,
                    StartDate = o.StartDate,
                    Title = o.Title,
                    Distance = ProcessDistance(GetDistance(longitude, latitude, o.Longitude, o.Latitude)),
                    OrganizationLogoUrl = logoImgUrl,
                    CreatorName = o.CreatorName,
                    CreatorPortraitUrl = o.CreatorPortraitUrl
                });
            }
            return topics;
        }

        public async Task<IEnumerable<TopicDTO>> GetTopicsByOrganizationId(int pageIndex, int pageSize, string organizationId, string key = "")
        {
            var result = _topicRepository.GetFiltered(o => o.OrganizationId == organizationId && (string.IsNullOrEmpty(key) || o.Title.Contains(key)))
                .OrderByDescending(g => g.CreatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(item =>
                {
                    return new TopicDTO
                    {
                        Address = item.Address,
                        Category = item.Category,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        CustomTopicUrl = item.CustomTopicUrl,
                        EndDate = item.EndDate,
                        FaceImageUrl = item.FaceImageUrl,
                        Id = item.Id,
                        Introduction = item.Introduction,
                        IsCustom = item.IsCustom,
                        IsSuspend = item.IsSuspend,
                        LastUpdBy = item.LastUpdBy,
                        LastUpdOn = item.LastUpdOn,
                        OrganizationId = item.OrganizationId,
                        OrganizationName = "",
                        StartDate = item.StartDate,
                        Title = item.Title,
                        Distance = "",
                        CreatorName = item.CreatorName,
                        CreatorPortraitUrl = item.CreatorPortraitUrl
                    };
                })
                .ToList();

            return result;
        }

        public async Task UpdateTopic(TopicDTO model, string operatorId)
        {
            var obj = _topicRepository.Get(model.Id);
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
            obj.Introduction = model.Introduction;
            obj.LastUpdBy = operatorId;
            obj.LastUpdOn = DateTime.Now;
            obj.Title = model.Title;
            obj.CustomTopicUrl = model.CustomTopicUrl;
            obj.EndDate = model.EndDate;
            obj.StartDate = model.StartDate;
            obj.IsCustom = model.IsCustom;
            obj.FaceImageUrl = model.FaceImageUrl;

            _topicRepository.Update(obj);

            var imgs = _topicImageRepository.GetFiltered(o => o.TopicId == obj.Id)?.ToList();

            imgs?.ForEach(item =>
            {
                _topicImageRepository.Remove(item);
            });

            var i = 0;
            model.ContentImages?.ToList()?.ForEach(item =>
            {
                i++;
                _topicImageRepository.Add(new TopicImage { ImageId = item.ImageId, ImageUrl = item.ImageUrl, TopicId = obj.Id, Order = i });
            });

            _dbUnitOfWork.Commit();

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

        string ProcessDistance(double distance)
        {
            if (distance < 1000) return $"{String.Format("{0:N0} ", distance)}m";
            return $"{String.Format("{0:N2} ", distance / 1000)}km";
        }
        #endregion

    }
}
