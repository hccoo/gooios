using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.DTOs
{
    public class ServiceDTO
    {
        public string Id { get; set; }

        public string VideoUrl { get; set; }

        public string IOSVideoUrl { get; set; }


        public string GoodsCategoryName { get; set; }

        public string Title { get; set; }

        public string Introduction { get; set; }

        /// <summary>
        /// 诚意金
        /// </summary>
        public decimal SincerityGold { get; set; }

        /// <summary>
        /// 服务范围,0为不限制，如 3 表示3公里以内
        /// </summary>
        public int ServeScope { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        /// <summary>
        /// 服务的驻地，即服务人员从哪里出发
        /// </summary>
        public Address Station { get; set; }

        public ServiceStatus Status { get; set; }

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationLogoUrl { get; set; }

        public string ServicePhone { get; set; }

        public string Distance { get; set; }

        public bool IsAdvertisement { get; set; } = false;

        public string PersonalizedPageUri { get; set; } = null;

        public IEnumerable<ServiceImageDTO> Images { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        public IEnumerable<SimpleGoods> Goods { get; set; }
    }

    public class SimpleGoods
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal UnitPrice { get; set; }
        
        public string GoodsCategoryName { get; set; }

        public string Category { get; set; }

        /// <summary>
        /// 购买单位，比如：件,块,平方米，米等等
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 推荐商品排序 6，5，4，3，2，1
        /// </summary>
        public int RecommendLevel { get; set; } = 0;

        /// <summary>
        /// 热门排序 
        /// </summary>
        public int Order { get; set; } = 0;
        /// <summary>
        /// 货号
        /// </summary>
        public string ItemNumber { get; set; }

        public string OrganizationId { get; set; }

    }
}
