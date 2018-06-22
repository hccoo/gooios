using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class ReservationModel
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

        public ServiceModel Service { get; set; }

        public string ServicerId { get; set; }

        public ServicerModel Servicer { get; set; }

        public decimal SincerityGoldNeedToPay { get; set; }

        public ReservationStatus Status { get; set; }
    }
    public enum ReservationStatus
    {
        Submitted = 1,
        Underway = 2,//进行中
        Completed = 9,
        Failed = 4,
        Cancelled = 7
    }
}
