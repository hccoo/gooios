using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Domains.Aggregates
{
    public static class OrganizationFactory
    {
        public static Organization CreateOrganization(
            string fullName, 
            string shortName,
            string certificateNo,
            string introduction,
            string operatorId,
            Address address,
            string logoImageId,
            string phone,
            string customServicePhone)
        {
            var result = new Organization();
            result.GenerateId();

            var now = DateTime.Now;

            result.FullName = fullName;
            result.ShortName = shortName;
            result.Address = address;
            result.CertificateNo = certificateNo;
            result.Introduction = introduction;
            result.LastUpdBy = operatorId;
            result.CreatedBy = operatorId;
            result.CreatedOn = now;
            result.LastUpdOn = now;
            result.LogoImageId = logoImageId;
            result.InitAddress();
            result.Phone = phone;
            result.CustomServicePhone = customServicePhone;

            return result;
        }
    }
}
