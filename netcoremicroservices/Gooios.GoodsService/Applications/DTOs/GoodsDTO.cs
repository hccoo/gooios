using Gooios.GoodsService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class GoodsDTO
    {
        public string Id { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ItemNumber { get; set; }

        public string VideoPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        /// <summary>
        /// 本平台单件购买价格
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 市场参考价格
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 购买单位，比如：件,块,平方米，米等等
        /// </summary>
        public string Unit { get; set; }

        public GoodsStatus Status { get; set; }

        /// <summary>
        /// 库存，单位为Unit
        /// </summary>
        public int Stock { get; set; }

        public string StoreId { get; set; } = "2877578C-4518-4BD4-A6ED-D84DD0BEEDDD"; //前期默认ArcaneStars自营

        public IEnumerable<GrouponConditionDTO> GrouponConditions { get; set; }

        public IEnumerable<GoodsImageDTO> GoodsImages { get; set; }

        /// <summary>
        /// 表示用户在购买商品时可以选择的属性标签，比如：颜色:红色、绿色，尺寸：XL\L\M\S等等 对应值：
        /// [
        ///     {
        ///         Name:"颜色", 
        ///         Values:["红色","绿色"]
        ///     },
        ///     {
        ///         Name:"尺寸",
        ///         Values:["XL","L","M","S"]
        ///     }
        /// ]
        /// </summary>
        public string OptionalPropertyJsonObject { get; set; }

        public Address Address { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        public string Distance { get; set; }

        /// <summary>
        /// 配送范围,0为不限制，如 3 表示3公里以内
        /// </summary>
        public int DistributionScope { get; set; } = 0;

        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationLogoUrl { get; set; }
        
    }

    public class GoodsIdDTO
    {
        public string Id { get; set; }
    }
}
