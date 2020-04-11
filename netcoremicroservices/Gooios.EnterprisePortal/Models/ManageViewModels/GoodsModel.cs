using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class GoodsModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ItemNumber { get; set; }

        public string Title { get; set; }

        public string GoodsCategoryName { get; set; }

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

        public string StoreId { get; set; } 

        public IEnumerable<GrouponCondition> GrouponConditions { get; set; }

        public IEnumerable<GoodsImage> GoodsImages { get; set; }

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

        public string OrganizationId { get; set; }

        /// <summary>
        /// 配送范围,0为不限制，如 3 表示3公里以内
        /// </summary>
        public int DistributionScope { get; set; } = 0;

    }

    public class GoodsId
    {
        public string Id { get; set; }
    }
    public enum GoodsStatus
    {
        //初稿
        Draft = 1,
        //待上架（已申请上架）
        Shelving = 2,
        //已上架
        Shelved = 3,
        //下架（不能交易，但能看见）
        UnShelved = 4,
        //禁止（不能交易，并且不能看见）
        Suspend = 5,
        //已售完
        SoldOut = 6
    }
    public class GrouponCondition
    {
        public int Id { get; set; }

        public string GoodsId { get; set; }

        public int MoreThanNumber { get; set; }

        public decimal Price { get; set; }
    }
    public class GoodsImage
    {
        public int Id { get; set; }

        public string GoodsId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
