using Gooios.OrderService.Domains;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Applications.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }

        public string OrderNo { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal ShippingCost { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal PreferentialAmount { get; set; }

        /// <summary>
        /// 税费
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// 订单支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public OrderStatus Status { get; set; }

        public InvoiceType InvoiceType { get; set; }
        
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
        
        public Address CustomerAddress { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Service or Goods
        /// </summary>
        public string Mark { get; set; }

        public string OrganizationId { get; set; }

        public string InvoiceRemark { get; set; }

        public string Remark { get; set; }

        public string ActivityId { get; set; }
    }
}
