using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public static class DeliveryNoteFactory
    {
        public static DeliveryNote CreateDeliveryNote(
            Order order,
            string deliveryNoteNo,
            string consignee,
            string consigneeMobile,
            string deliveryAddress,
            ShippingMethod shippingMethod,
            string carrierName,
            string carrierPhone,
            decimal shippingAmount,
            string operatorId
            )
        {
            var result = new DeliveryNote
            {
                CarrierName = carrierName,
                CarrierPhone = carrierPhone,
                ShippingMethod = shippingMethod,
                Consignee = consignee,
                ConsigneeMobile = consigneeMobile,
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                DeliveryAddress = deliveryAddress,
                DeliveryNoteNo = deliveryNoteNo,
                OrderId = order.Id,
                ShippingAmount = shippingAmount
            };

            result.GenerateId();

            return result;
        }
    }
}
