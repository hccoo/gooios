using Gooios.FancyService.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class Service : Entity<string>
    {
        public string Title { get; set; }

        public string Introduction { get; set; }

        /// <summary>
        /// 商品品类名称
        /// </summary>
        public string GoodsCategoryName { get; set; }

        public string VideoUrl { get; set; }

        public string IOSVideoUrl { get; set; }

        /// <summary>
        /// 诚意金
        /// </summary>
        public decimal SincerityGold { get; set; }

        //public string ServicerId { get; set; }

        /// <summary>
        /// 服务范围,0为不限制，如 3 表示3公里以内
        /// </summary>
        public int ServeScope { get; set; }
        
        public string Category { get; set; }

        public string SubCategory { get; set; }

        /// <summary>
        /// 服务的驻地，即服务人员从哪里出发
        /// </summary>
        [NotMapped]
        public Address Station { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ServiceStatus Status { get; set; }

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastUpdBy { get; set; }

        public DateTime LastUpdOn { get; set; }

        public bool IsAdvertisement { get; set; } = false;

        public string PersonalizedPageUri { get; set; } = null;

        public string ApplicationId { get; set; } = "GOOIOS001";

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void InitStatus()
        {
            Status = ServiceStatus.Normal;
        }

        public void SetSuspend()
        {
            Status = ServiceStatus.Suspended;
        }
        public void SetDeleted()
        {
            Status = ServiceStatus.Deleted;
        }

        public void InitAddress()
        {
            Area = Station.Area;
            City = Station.City;
            Postcode = Station.Postcode;
            Province = Station.Province;
            StreetAddress = Station.StreetAddress;
            Latitude = Station.Latitude;
            Longitude = Station.Longitude;
        }

        public void ResolveAddress()
        {
            if (Station == null) Station = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }
    }

    public enum ServiceStatus
    {
        Suspended = 0,
        Normal = 1,
        Deleted = 4
    }
}
