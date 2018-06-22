using System;

namespace Gooios.FancyService.Domains.Aggregates
{
    public static class ReservationFactory
    {
        public static Reservation CreateInstance(
            Service service,
            Servicer servicer,
            Address serviceDestination,
            string customerName,
            string customerMobile,
            DateTime appointTime,
            decimal sincerityGoldNeedToPay,
            string operatorId)
        {
            var result = new Reservation
            {
                ReservationNo = ReservationNoGenerator.GenerateOrderNo(),
                AppointTime = appointTime,
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                CustomerMobile = customerMobile,
                CustomerName = customerName,
                LastUpdBy = operatorId,
                LastUpdOn = DateTime.Now,
                Service = service,
                ServiceDestination = serviceDestination,
                ServiceId = service?.Id,
                Servicer = servicer,
                ServicerId = servicer?.Id,
                SincerityGoldNeedToPay = sincerityGoldNeedToPay,
                OrganizationId = service?.OrganizationId ?? servicer?.OrganizationId
            };
            result.GenerateId();
            result.InitStatus();
            result.InitAddress();

            return result;
        }
    }
}
