using Gooios.Infrastructure;
using Gooios.OrganizationService.Applications.DTOs;
using Gooios.OrganizationService.Domains.Aggregates;
using Gooios.OrganizationService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Applications.Services
{
    public interface IOrganizationAppService : IApplicationServiceContract
    {
        void AddOrganization(OrganizationDTO model, string operatorId);

        void UpdateOrganization(OrganizationDTO model, string operatorId);

        OrganizationDTO GetOrganizationById(string id);

        IEnumerable<OrganizationDTO> GetNearbyOrganizations(double longitude, double latitude, int pageIndex, int pageSize);
    }

    public class OrganizationAppService : ApplicationServiceContract, IOrganizationAppService
    {
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IOrganizationRepository _organizationRepository;

        public OrganizationAppService(IDbUnitOfWork dbUnitOfWork, IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void AddOrganization(OrganizationDTO model, string operatorId)
        {
            var organization = OrganizationFactory.CreateOrganization(model.FullName, model.ShortName, model.CertificateNo, model.Introduction, operatorId, model.Address, model.LogoImageId, model.Phone, model.CustomServicePhone);
            _organizationRepository.Add(organization);
            _dbUnitOfWork.Commit();
        }

        public void UpdateOrganization(OrganizationDTO model, string operatorId)
        {
            var result = _organizationRepository.Get(model.Id);

            if (result == null) return; //TODO: update this logic

            result.Address = model.Address;
            result.InitAddress();

            result.CertificateNo = model.CertificateNo;
            result.FullName = model.FullName;
            result.Introduction = model.Introduction;
            result.IsSuspend = model.IsSuspend;
            result.LastUpdBy = operatorId;
            result.LastUpdOn = DateTime.Now;
            result.ShortName = model.ShortName;
            result.Phone = model.Phone;
            result.CustomServicePhone = model.CustomServicePhone;

            _organizationRepository.Update(result);
            _dbUnitOfWork.Commit();
        }

        public IEnumerable<OrganizationDTO> GetNearbyOrganizations(double longitude, double latitude, int pageIndex, int pageSize)
        {
            return _organizationRepository.GetPaged(pageIndex, pageSize, o => GetDistance(longitude, latitude, o.Longitude, o.Latitude), true).Select(item =>
            {
                item.ResolveAddress();
                return new OrganizationDTO
                {
                    Address = item.Address,
                    CertificateNo = item.CertificateNo,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    FullName = item.FullName,
                    Id = item.Id,
                    Introduction = item.Introduction,
                    LastUpdBy = item.LastUpdBy,
                    LastUpdOn = item.LastUpdOn,
                    ShortName = item.ShortName,
                    Phone = item.Phone,
                    CustomServicePhone = item.CustomServicePhone,
                    LogoImageId = item.LogoImageId,
                    IsSuspend = item.IsSuspend
                };
            });

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

        public OrganizationDTO GetOrganizationById(string id)
        {
            var result = _organizationRepository.Get(id);
            result.ResolveAddress();
            return new OrganizationDTO
            {
                Address = result.Address,
                CertificateNo = result.CertificateNo,
                CreatedBy = result.CreatedBy,
                CreatedOn = result.CreatedOn,
                FullName = result.FullName,
                Id = result.Id,
                Introduction = result.Introduction,
                LastUpdBy = result.LastUpdBy,
                LastUpdOn = result.LastUpdOn,
                ShortName = result.ShortName,
                Phone = result.Phone,
                CustomServicePhone = result.CustomServicePhone,
                LogoImageId = result.LogoImageId
            };
        }
    }
}
