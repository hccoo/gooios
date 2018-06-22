using Gooios.OrderService.Domains;
using Gooios.OrderService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Applications.DTOs
{
    public class DeliveryNoteDTO
    {
        public string Id { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string DeliveryNoteNo { get; set; }

        public string OrderId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string DeliveryAddress { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        public string CarrierName { get; set; }

        /// <summary>
        /// 承运商电话
        /// </summary>
        public string CarrierPhone { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal ShippingAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
        
    }
}
