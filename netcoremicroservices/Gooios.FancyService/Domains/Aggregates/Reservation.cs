using Gooios.FancyService.Domains;
using Gooios.FancyService.Domains.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class Reservation : Entity<string>
    {
        public string ReservationNo { get; set; }

        /// <summary>
        /// 客户要求的服务服务目的地
        /// </summary>
        [NotMapped]
        public Address ServiceDestination { get; set; }

        #region Address
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        #endregion

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointTime { get; set; }

        public string ServiceId { get; set; }

        [NotMapped]
        public Service Service { get; set; }

        public string ServicerId { get; set; }

        [NotMapped]
        public Servicer Servicer { get; set; }

        public decimal SincerityGoldNeedToPay { get; set; }

        public ReservationStatus Status { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastUpdBy { get; set; }

        public DateTime LastUpdOn { get; set; }

        public string OrganizationId { get; set; }

        public string OrderId { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void InitAddress()
        {
            Area = ServiceDestination.Area;
            City = ServiceDestination.City;
            Postcode = ServiceDestination.Postcode;
            Province = ServiceDestination.Province;
            StreetAddress = ServiceDestination.StreetAddress;
            Latitude = ServiceDestination.Latitude;
            Longitude = ServiceDestination.Longitude;
        }

        public void ResolveAddress()
        {
            if (ServiceDestination == null) ServiceDestination = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }

        public void InitStatus()
        {
            Status = ReservationStatus.Submitted;
        }

        public void SetUnderwayStatus()
        {
            Status = ReservationStatus.Underway;
        }

        public void SetCompletedStatus()
        {
            Status = ReservationStatus.Completed;
        }

        public void SetFailedStatus()
        {
            Status = ReservationStatus.Failed;
        }

        public void SetCancelledStatus()
        {
            Status = ReservationStatus.Cancelled;
        }

        public void ConfirmAppoint()
        {
            DomainEvent.Publish(new AppointmentConfirmedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now,  ConfirmedTime = DateTime.Now });
        }

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
