using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public static class GoodsFactory
    {
        public static Goods CreateGoods(string itemNumber,
            string category,
            string subCategory,
            string title,
            string description,
            string detail,
            decimal unitPrice,
            decimal marketPrice,
            string unit,
            int stock,
            string storeId,
            string operatorId,
            string optionalProperty,
            IEnumerable<GrouponCondition> grouponConditions,
            IEnumerable<GoodsImage> goodsImages, //, GoodsNumber goodsNumber
            Address address,
            int distributionScope,
            string goodsCategoryName,
            string videoPath=""
            )
        {
            var result = new Goods
            {
                CreatedBy = operatorId,
                CreatedOn = DateTime.Now,
                Description = description,
                Detail = detail,
                GoodsImages = goodsImages,
                GrouponConditions = grouponConditions,
                MarketPrice = marketPrice,
                OptionalPropertyJsonObject = optionalProperty,
                Stock = stock,
                StoreId = storeId,
                Title = title,
                Unit = unit,
                UnitPrice = unitPrice,
                ItemNumber = itemNumber,
                Category = category,
                SubCategory = subCategory,
                LastUpdBy = operatorId,
                LastUpdOn = DateTime.Now,
                Address = address,
                DistributionScope = distributionScope,
                VideoPath= videoPath,
                GoodsCategoryName = goodsCategoryName
            };

            result.GenerateId();
            result.InitAddress();

            grouponConditions?.ToList().ForEach(item =>
            {
                item.GoodsId = result.Id;
            });

            goodsImages?.ToList().ForEach(item =>
            {
                item.GoodsId = result.Id;
            });

            result.InitStatus();

            return result;
        }
    }
}
