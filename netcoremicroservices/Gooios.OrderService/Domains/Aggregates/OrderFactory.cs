using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public static class OrderFactory
    {
        public static Order CreateOrder(
            decimal totalAmount,
            decimal shippingCost,
            decimal preferrentialAmount,
            decimal tax,
            decimal payAmount,
            string customerName,
            string customerMobile,
            InvoiceType invoiceType,
            Address address,
            IEnumerable<OrderItem> orderItems,
            string operatorId,
            string mark,
            string organizationId,
            string invoiceRemark,
            string remark,
            string activityId,
            string title = ""
            )
        {
            var result = new Order
            {
                TotalAmount = totalAmount,
                ShippingCost = shippingCost,
                PreferentialAmount = preferrentialAmount,
                Tax = tax,
                PayAmount = payAmount,
                CustomerName = customerName,
                CustomerMobile = customerMobile,
                InvoiceType = invoiceType,
                CustomerAddress = address,
                OrderItems = orderItems,
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                UpdatedBy = operatorId,
                UpdatedOn = DateTime.Now,
                Mark = mark,
                OrganizationId = organizationId,
                InvoiceRemark = invoiceRemark,
                Remark = remark,
                ActivityId = activityId,
                Title = title
            };
            result.GenerateId();
            result.InitAddress();
            result.GenerateOrderNo();
            result.InitStatus();

            return result;
        }
    }
}
