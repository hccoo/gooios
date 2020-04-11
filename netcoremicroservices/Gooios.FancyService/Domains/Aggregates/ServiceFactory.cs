using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public static class ServiceFactory
    {
        public static Service CreateInstance(
            string title,
            string introduction,
            decimal sincerityGold,
            int serveScope,
            string category,
            string subCategory,
            Address station,
            string organizationId,
            string operatorId,
            string videoUrl,
            string goodsCategoryName,
            string applicationId = "GOOIOS001")
        {
            var result = new Service
            {
                Category = category,
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                Introduction = introduction,
                LastUpdBy = operatorId,
                LastUpdOn = DateTime.Now,
                OrganizationId = organizationId,
                ServeScope = serveScope,
                SincerityGold = sincerityGold,
                Station = station,
                SubCategory = subCategory,
                Title = title,
                ApplicationId = applicationId,
                VideoUrl = videoUrl,
                GoodsCategoryName = goodsCategoryName
            };

            result.GenerateId();
            result.InitAddress();
            result.InitStatus();

            return result;
        }

    }
}
