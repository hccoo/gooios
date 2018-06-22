using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.DTOs
{
    public class ReservationDTO
    {
        public string Id { get; set; }

        public string ReservationNo { get; set; }

        /// <summary>
        /// 客户要求的服务服务目的地
        /// </summary>
        public Address ServiceDestination { get; set; }
        
        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointTime { get; set; }

        public string ServiceId { get; set; }
        
        public ServiceDTO Service { get; set; }

        public string ServicerId { get; set; }
        
        public ServicerDTO Servicer { get; set; }

        public decimal SincerityGoldNeedToPay { get; set; }

        public ReservationStatus Status { get; set; }

        public string OrganizationId { get; set; }

        public string OrderId { get; set; }
    }
    
}
