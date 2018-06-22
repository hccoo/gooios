using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies.DTOs
{
    public class OrderDTO
    {
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
        
        public InvoiceType InvoiceType { get; set; }

        public IEnumerable<OrderItemDTO> OrderItems { get; set; }

        public Address CustomerAddress { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Mark { get; set; }

        public string OrganizationId { get; set; }

        public string Invoiceremark { get; set; }

        public string Remark { get; set; }

    }
    public class OrderItemDTO
    {
        public int Id { get; set; }

        public string OrderId { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// GoodsId or ServiceId and so on.
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// GoodsNo or ServiceNo and so on.
        /// </summary>
        public string ObjectNo { get; set; }

        public string PreviewPictureUrl { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// 成交单价
        /// </summary>
        public decimal TradeUnitPrice { get; set; }

        /// <summary>
        /// eg: 
        /// [
        ///     {
        ///         Name:"颜色",
        ///         Value:"红色"
        ///     },
        ///     {
        ///          Name:"尺寸",
        ///          Value:"XL"
        ///     }
        /// ]     
        /// </summary>
        public string SelectedProperties { get; set; }
    }

    public enum InvoiceType
    {
        VAT = 1,  //赠票
        Common = 2,//普票
        None = 3 //不开发票
    }
}
