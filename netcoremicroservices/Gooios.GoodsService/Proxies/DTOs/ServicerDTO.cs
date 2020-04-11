using Gooios.GoodsService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class ServicerDTO
    {
        public string Id { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 技术职称
        /// </summary>
        public string TechnicalTitle { get; set; }

        /// <summary>
        /// 技能等级
        /// </summary>
        public Grade TechnicalGrade { get; set; }

        public Gender Gender { get; set; }

        /// <summary>
        /// 开始从事相关工作时间
        /// </summary>
        public DateTime StartRelevantWorkTime { get; set; }

        public DateTime BirthDay { get; set; }
        
        public Address Address { get; set; }

        public string PersonalIntroduction { get; set; }

        /// <summary>
        /// 最终下单Service的SincerityGold*SincerityGoldRate为需要支付的最终诚意金
        /// </summary>
        public double SincerityGoldRate { get; set; }

        public bool IsSuspend { get; set; }

        public string PortraitImageId { get; set; }
        public string PortraitImageUrl { get; set; }

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        public IEnumerable<ServicerImageDTO> Images { get; set; }
        
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public decimal SincerityGold { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        public string UserName { get; set; }
    }


    public enum Grade
    {
        Primary = 1,
        Middle = 2,
        Advanced = 3,
        Senior = 4,
        Expert = 5, //专家级
        Master = 6   //大师级
    }

    public enum Gender
    {
        UnKnow = 0,
        Male = 1,
        Woman = 2
    }
    public class ServicerImageDTO
    {
        public int Id { get; set; }

        public string ServicerId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
