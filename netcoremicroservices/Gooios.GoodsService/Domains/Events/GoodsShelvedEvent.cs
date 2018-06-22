using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Events
{
    public class GoodsShelvedEvent : DomainEvent
    {
        public GoodsShelvedEvent() { }
        public GoodsShelvedEvent(IEntity source) : base(source) { }

        public DateTime ShelvedTime { get; set; }
    }

    //[HandlesAsynchronouslyAttribute]
    public class GoodsShelvedEventHandler : IDomainEventHandler<GoodsShelvedEvent>
    {
        private readonly IEventBus _bus;
        private readonly IOnlineGoodsRepository _onlineGoodsRepository;
        private readonly IOnlineGoodsImageRepository _onlineGoodsImageRepository;
        private readonly IOnlineGrouponConditionRepository _onlineGrouponConditionRepository;
        private readonly IDbUnitOfWork _dbUnitOfWork;

        public GoodsShelvedEventHandler(IEventBus bus, IOnlineGoodsRepository onlineGoodsRepository, IOnlineGoodsImageRepository onlineGoodsImageRepository, IOnlineGrouponConditionRepository onlineGrouponConditionrepository, IDbUnitOfWork dbUnitOfWork)
        {
            _bus = bus;
            _onlineGoodsRepository = onlineGoodsRepository;
            _onlineGoodsImageRepository = onlineGoodsImageRepository;
            _onlineGrouponConditionRepository = onlineGrouponConditionrepository;
            _dbUnitOfWork = dbUnitOfWork;
        }
        public void Handle(GoodsShelvedEvent evnt)
        {
            var eventSource = evnt.Source as Goods;

            var goods = _onlineGoodsRepository.Get(eventSource.Id);

            if (goods == null)
            {
                var obj = new OnlineGoods
                {
                    CreatedBy = eventSource.CreatedBy,
                    CreatedOn = eventSource.CreatedOn,
                    Description = eventSource.Description,
                    Detail = eventSource.Detail,
                    ItemNumber = eventSource.ItemNumber,
                    LastUpdBy = eventSource.LastUpdBy,
                    LastUpdOn = eventSource.LastUpdOn,
                    MarketPrice = eventSource.MarketPrice,
                    OptionalPropertyJsonObject = eventSource.OptionalPropertyJsonObject,
                    Stock = eventSource.Stock,
                    StoreId = eventSource.StoreId,
                    Title = eventSource.Title,
                    Unit = eventSource.Unit,
                    UnitPrice = eventSource.UnitPrice,
                    Category = eventSource.Category,
                    SubCategory = eventSource.SubCategory,
                    DistributionScope = eventSource.DistributionScope,
                    Address = eventSource.Address
                };

                obj.InitAddress();
                obj.SetId(eventSource.Id);
                obj.InitStatus();

                _onlineGoodsRepository.Add(obj);

                foreach (var item in eventSource.GoodsImages)
                {
                    _onlineGoodsImageRepository.Add(new OnlineGoodsImage { CreatedOn = item.CreatedOn, GoodsId = item.GoodsId, ImageId = item.ImageId });
                }

                foreach (var item in eventSource.GrouponConditions)
                {
                    _onlineGrouponConditionRepository.Add(new OnlineGrouponCondition { GoodsId = item.GoodsId, MoreThanNumber = item.MoreThanNumber, Price = item.Price });
                }
                _dbUnitOfWork.Commit();
            }
            else
            {
                goods.Description = eventSource.Description;
                goods.Detail = eventSource.Detail;
                goods.ItemNumber = eventSource.ItemNumber;
                goods.LastUpdBy = eventSource.LastUpdBy;
                goods.LastUpdOn = eventSource.LastUpdOn;
                goods.MarketPrice = eventSource.MarketPrice;
                goods.OptionalPropertyJsonObject = eventSource.OptionalPropertyJsonObject;
                goods.Stock = eventSource.Stock;
                //goods.StoreId = eventSource.StoreId;
                goods.Title = eventSource.Title;
                goods.Unit = eventSource.Unit;
                goods.UnitPrice = eventSource.UnitPrice;
                goods.Category = eventSource.Category;
                goods.SubCategory = eventSource.SubCategory;
                goods.Address = eventSource.Address;
                goods.InitAddress();

                goods.InitStatus();

                _onlineGoodsRepository.Update(goods);

                var imgs = _onlineGoodsImageRepository.GetFiltered(o => o.GoodsId == eventSource.Id).ToList();
                imgs.ForEach(img => { _onlineGoodsImageRepository.Remove(img); });

                var conditions = _onlineGrouponConditionRepository.GetFiltered(o => o.GoodsId == eventSource.Id).ToList();
                conditions.ForEach(condition => { _onlineGrouponConditionRepository.Remove(condition); });

                foreach (var item in eventSource.GoodsImages)
                {
                    _onlineGoodsImageRepository.Add(new OnlineGoodsImage { CreatedOn = item.CreatedOn, GoodsId = item.GoodsId, ImageId = item.ImageId });
                }

                foreach (var item in eventSource.GrouponConditions)
                {
                    _onlineGrouponConditionRepository.Add(new OnlineGrouponCondition { GoodsId = item.GoodsId, MoreThanNumber = item.MoreThanNumber, Price = item.Price });
                }

                _dbUnitOfWork.Commit();
            }

            _bus.Publish(evnt);
        }
    }
}
