using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public static class ServicerFactory
    {
        public static Servicer CreateInstance(
            string jobNumber,
            string name,
            string technicalTitle,
            Grade technicalGrade,
            Gender gender,
            DateTime birthDay,
            Address address,
            string personalIntroduction,
            double sincerityGoldRate,
            string operatorId,
            bool isSuspend,
            string organizationId,
            string category,
            string subCategory,
            string portraitImageId,
            DateTime startRelevantWorkTime,
            decimal sincerityGold,
            string userName,
            string applicationId = "GOOIOS001"
            )
        {
            var result = new Servicer
            {
                Address = address,
                BirthDay = birthDay,
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                Gender = gender,
                JobNumber = jobNumber,
                LastUpdBy = operatorId,
                LastUpdOn = DateTime.Now,
                Name = name,
                PersonalIntroduction = personalIntroduction,
                SincerityGoldRate = sincerityGoldRate,
                TechnicalGrade = technicalGrade,
                TechnicalTitle = technicalTitle,
                IsSuspend = isSuspend,
                OrganizationId = organizationId,
                Category = category,
                SubCategory = subCategory,
                PortraitImageId = portraitImageId,
                StartRelevantWorkTime = startRelevantWorkTime,
                SincerityGold = sincerityGold,
                ApplicationId = applicationId,
                UserName = userName
            };

            result.GenerateId();
            result.InitAddress();

            return result;
        }
    }
}
