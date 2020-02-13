using Gooios.GoodsService.Domains;
using Gooios.GoodsService.Domains.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public class OnlineGoods : Entity<string>
    {

        //public GoodsNumber GoodsNumber { get; set; }

        public string ItemNumber { get; set; }

        public string Title { get; set; }

        public string VideoPath { get; set; }

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

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdOn { get; set; }

        public string LastUpdBy { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        /// <summary>
        /// 配送范围,0为不限制，如 3 表示3公里以内
        /// </summary>
        public int DistributionScope { get; set; } = 0;

        [NotMapped]
        public IEnumerable<OnlineGrouponCondition> GrouponConditions { get; set; }
        [NotMapped]
        public IEnumerable<OnlineGoodsImage> GoodsImages { get; set; }

        public void InitAddress()
        {
            Area = Address.Area;
            City = Address.City;
            Postcode = Address.Postcode;
            Province = Address.Province;
            StreetAddress = Address.StreetAddress;
            Latitude = Address.Latitude;
            Longitude = Address.Longitude;
        }

        public void ResolveAddress()
        {
            if (Address == null) Address = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }

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

        public void SetId(string id)
        {
            Id = id;
        }

        public void InitStatus()
        {
            Status = GoodsStatus.Shelved;
        }

        public void SetUnShelved()
        {
            Status = GoodsStatus.UnShelved;
        }

        public void SetSuspend()
        {
            Status = GoodsStatus.Suspend;
        }

        public void SetSoldOut()
        {
            Stock = 0;
            Status = GoodsStatus.SoldOut;
        }

        public void SoldConfirm()
        {
            Stock--;
            if (Stock <= 0)
            {
                DomainEvent.Publish(new GoodsSoldOutEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, SoldOutTime = DateTime.Now });
            }
        }
    }
}
