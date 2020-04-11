using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public static class OnlineGoodsFactory
    {
        public static OnlineGoods CreateGoods(Goods goods)
        {
            var result = new OnlineGoods
            {
                CreatedBy = goods.CreatedBy,
                CreatedOn = DateTime.Now,
                Description = goods.Description,
                Detail = goods.Detail,
                MarketPrice = goods.MarketPrice,
                OptionalPropertyJsonObject = goods.OptionalPropertyJsonObject,
                Stock = goods.Stock,
                Title = goods.Title,
                Unit = goods.Unit,
                UnitPrice = goods.UnitPrice,
                StoreId = goods.StoreId,
                VideoPath = goods.VideoPath,
                GoodsCategoryName = goods.GoodsCategoryName
            };

            result.GrouponConditions = goods.GrouponConditions.Select(item =>
            {
                return new OnlineGrouponCondition
                {
                    GoodsId = item.GoodsId,
                    MoreThanNumber = item.MoreThanNumber,
                    Price = item.Price
                };
            });

            result.GoodsImages = goods.GoodsImages.Select(item =>
            {
                return new OnlineGoodsImage
                {
                    GoodsId = item.GoodsId,
                    CreatedOn = DateTime.Now,
                    ImageId = item.ImageId
                };
            });

            result.SetId(goods.Id);
            result.InitStatus();

            return result;
        }
    }
}
