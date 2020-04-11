using Gooios.FancyService.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class Servicer : Entity<string>
    {
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 服务者关联用户账号
        /// </summary>
        public string UserName { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 技术职称
        /// </summary>
        public string TechnicalTitle { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        /// <summary>
        /// 技能等级
        /// </summary>
        public Grade TechnicalGrade { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDay { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        #region address
        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        #endregion

        public string PersonalIntroduction { get; set; }

        /// <summary>
        /// 最终下单Service的SincerityGold*SincerityGoldRate为需要支付的最终诚意金
        /// </summary>
        public double SincerityGoldRate { get; set; }

        /// <summary>
        /// 诚意金,如果没有对应的服务或者SincerityGoldRate为0，那么以诚意金为准
        /// </summary>
        public decimal SincerityGold { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastUpdBy { get; set; }

        public DateTime LastUpdOn { get; set; }

        public bool IsSuspend { get; set; }

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public string PortraitImageId { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        /// <summary>
        /// 开始从事相关工作时间
        /// </summary>
        public DateTime StartRelevantWorkTime { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
        public void SetSuspend()
        {
            IsSuspend = true;
        }
        public void InitAddress()
        {
            Area = Address.Area;
            City = Address.City;
            Postcode = Address.Postcode;
            Province = Address.Province;
            StreetAddress = Address.StreetAddress;
            Latitude = Address.Latitude;
            Longitude = Address.Longitude;
        }

        public void ResolveAddress()
        {
            if (Address == null) Address = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }
    }

    public enum Grade
    {
        Primary = 1,
        Middle = 2,
        Advanced = 3,
        Senior = 4,
        Expert=5, //专家级
        Master=6   //大师级
    }

    public enum Gender
    {
        UnKnow = 0,
        Male = 1,
        Woman = 2
    }
}
