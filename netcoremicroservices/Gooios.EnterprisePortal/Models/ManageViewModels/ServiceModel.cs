using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class ServiceModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string GoodsCategoryName { get; set; }

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

        public IEnumerable<ServiceImageDTO> Images { get; set; }
    }

    public enum ServiceStatus
    {
        Suspended = 0,
        Normal = 1,
        Deleted = 4
    }
    public class ServiceImageDTO
    {
        public int Id { get; set; }

        public string ServiceId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
