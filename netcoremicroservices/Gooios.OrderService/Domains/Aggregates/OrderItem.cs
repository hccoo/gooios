using Gooios.OrderService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{

    public class OrderItem : Entity<int>
    {
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

}
