using Autofac;
using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.GoodsService.Proxies;
using Gooios.GoodsService.Proxies.DTOs;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.Services
{
    public interface IGoodsAppService : IApplicationServiceContract
    {
        void AddGoods(GoodsDTO goodsDTO, string creatorId);

        void UpdateGoods(GoodsDTO goodsDTO, string operatorId);

        void ApplyForShelveGoods(string id, string operatorId);

        void ShelveGoods(string id, string operatorId);

        void UnShelveGoods(string id, string operatorId);

        void SuspendGoods(string id, string operatorId);

        Task<GoodsDTO> GetGoods(string id);

        Task<GoodsDTO> GetOnlineGoods(string id);

        Task<IEnumerable<GoodsDTO>> GetGoods(string key, string category, string subCategory, int pageIndex, int pageSize, string storeId = "");

        Task<IEnumerable<GoodsDTO>> GetOnlineGoods(string key, string category, string subCategory, int pageIndex, int pageSize);

        void SetStock(string goodsId, int increment, string operatorId);

        Task<IEnumerable<GoodsDTO>> GetNearbyGoods(double longitude, double latitude, string category, string subCategory, int pageIndex, int pageSize, string appId = "GOOIOS001");

        Task<string> ConfirmBuyGoods(ConfirmBuyGoodsDTO model, string operatorId);
    }

    public class GoodsAppService : ApplicationServiceContract, IGoodsAppService
    {
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly IGoodsRepository _goodsRepository;
        readonly IGoodsImageRepository _goodsImageRepository;
        readonly IGrouponConditionRepository _grouponConditionRepository;
        readonly IOnlineGoodsImageRepository _onlineGoodsImageRepository;
        readonly IOnlineGrouponConditionRepository _onlineGrouponConditionRepository;
        readonly IOnlineGoodsRepository _onlineGoodsRepository;
        readonly IImageServiceProxy _imageServiceProxy;
        readonly IOrganizationServiceProxy _organizationServiceProxy;
        readonly IAmapProxy _amapProxy;
        readonly IActivityServiceProxy _activityServiceProxy;
        readonly IOrderServiceProxy _orderServiceProxy;
        readonly IAuthServiceProxy _authServiceProxy;
        readonly TmpInstanceGenerate _tmp;

        public GoodsAppService(
            IDbUnitOfWork dbUnitOfWork,
            IEventBus eventBus,
            IGoodsRepository goodsRepository,
            IOnlineGoodsRepository onlineGoodsRepository,
            IGoodsImageRepository goodsImageRepository,
            IGrouponConditionRepository grouponConditionRepository,
            IOnlineGoodsImageRepository onlineGoodsImageRepository,
            IOnlineGrouponConditionRepository onlineGrouponConditionRepository,
            IImageServiceProxy imageServiceProxy,
            IOrganizationServiceProxy organizationServiceProxy,
            IAmapProxy amapProxy,
            IActivityServiceProxy activityServiceProxy,
            IOrderServiceProxy orderServiceProxy,
            IAuthServiceProxy authServiceProxy,
            TmpInstanceGenerate tmp)
        {
            _tmp = tmp;
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _goodsRepository = goodsRepository;
            _onlineGoodsRepository = onlineGoodsRepository;
            _goodsImageRepository = goodsImageRepository;
            _grouponConditionRepository = grouponConditionRepository;
            _onlineGoodsImageRepository = onlineGoodsImageRepository;
            _onlineGrouponConditionRepository = onlineGrouponConditionRepository;
            _imageServiceProxy = imageServiceProxy;
            _organizationServiceProxy = organizationServiceProxy;
            _amapProxy = amapProxy;
            _activityServiceProxy = activityServiceProxy;
            _orderServiceProxy = orderServiceProxy;
            _authServiceProxy = authServiceProxy;
        }

        public void AddGoods(GoodsDTO goodsDTO, string creatorId)
        {
            var goodsImages = goodsDTO.GoodsImages.Select(item => new GoodsImage { CreatedOn = DateTime.Now, ImageId = item.ImageId }).ToList();
            var grouponConditions = goodsDTO.GrouponConditions.Select(item => new GrouponCondition { MoreThanNumber = item.MoreThanNumber, Price = item.Price }).ToList();

            var obj = GoodsFactory.CreateGoods(
                goodsDTO.ItemNumber,
                goodsDTO.Category,
                goodsDTO.SubCategory,
                goodsDTO.Title,
                goodsDTO.Description,
                goodsDTO.Detail,
                goodsDTO.UnitPrice,
                goodsDTO.MarketPrice,
                goodsDTO.Unit,
                goodsDTO.Stock,
                goodsDTO.StoreId,
                creatorId,
                goodsDTO.OptionalPropertyJsonObject,
                grouponConditions,
                goodsImages,
                goodsDTO.Address,
                goodsDTO.DistributionScope);

            _goodsRepository.Add(obj);

            goodsImages.ForEach(item =>
            {
                _goodsImageRepository.Add(item);
            });

            grouponConditions.ForEach(item =>
            {
                _grouponConditionRepository.Add(item);
            });

            _dbUnitOfWork.Commit();
        }

        public void SetStock(string goodsId, int increment, string operatorId)
        {
            var goods = _goodsRepository.Get(goodsId);
            if (goods == null) throw new Exception("找不到指定的商品。");

            goods.IncrementStock(increment, operatorId);

            _goodsRepository.Update(goods);

            _dbUnitOfWork.Commit();
        }

        public void UpdateGoods(GoodsDTO goodsDTO, string operatorId)
        {
            var goods = _goodsRepository.Get(goodsDTO.Id);
            if (goods == null) return;
            goods.ItemNumber = goodsDTO.ItemNumber;
            goods.Category = goodsDTO.Category;
            goods.SubCategory = goodsDTO.SubCategory;
            goods.Title = goodsDTO.Title;
            goods.Description = goodsDTO.Description;
            goods.Detail = goodsDTO.Detail;
            goods.UnitPrice = goodsDTO.UnitPrice;
            goods.MarketPrice = goodsDTO.MarketPrice;
            goods.Unit = goodsDTO.Unit;
            goods.Stock = goodsDTO.Stock;
            goods.StoreId = goodsDTO.StoreId;
            goods.LastUpdBy = operatorId;
            goods.LastUpdOn = DateTime.Now;
            goods.OptionalPropertyJsonObject = goodsDTO.OptionalPropertyJsonObject;
            goods.Address = goodsDTO.Address;
            goods.DistributionScope = goodsDTO.DistributionScope;

            goods.InitAddress();
            goods.InitStatus();

            var imgs = _goodsImageRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();
            imgs.ForEach(img => { _goodsImageRepository.Remove(img); });

            var conditions = _grouponConditionRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();
            conditions.ForEach(condition => { _grouponConditionRepository.Remove(condition); });

            if (goodsDTO.GoodsImages != null && goodsDTO.GoodsImages.Count() > 0)
            {
                foreach (var item in goodsDTO.GoodsImages)
                {
                    _goodsImageRepository.Add(new GoodsImage { CreatedOn = DateTime.Now, GoodsId = goodsDTO.Id, ImageId = item.ImageId });
                }
            }

            if (goodsDTO.GrouponConditions != null && goodsDTO.GrouponConditions.Count() > 0)
            {
                foreach (var item in goodsDTO.GrouponConditions)
                {
                    _grouponConditionRepository.Add(new GrouponCondition { GoodsId = goodsDTO.Id, MoreThanNumber = item.MoreThanNumber, Price = item.Price });
                }
            }
            _dbUnitOfWork.Commit();
        }

        public void ApplyForShelveGoods(string id, string operatorId)
        {
            var goods = _goodsRepository.Get(id);
            if (goods != null)
            {
                goods.SetShelving();
                _goodsRepository.Update(goods);
                _dbUnitOfWork.Commit();
            }
        }

        public async Task<GoodsDTO> GetGoods(string id)
        {
            var obj = _goodsRepository.Get(id);

            if (obj != null)
            {
                var goodsImages = _goodsImageRepository.GetFiltered(o => o.GoodsId == id).Select(item => new GoodsImageDTO { GoodsId = item.GoodsId, Id = item.Id, ImageId = item.ImageId }).ToList();

                var imageIds = goodsImages.Select(o => o.ImageId).ToList();
                var images = await _imageServiceProxy.GetImagesByIds(imageIds);
                foreach (var img in goodsImages)
                {
                    var pic = images.FirstOrDefault(o => o.Id == img.ImageId);

                    img.HttpPath = pic?.HttpPath;
                    img.Title = pic?.Title;
                    img.Description = pic?.Description;
                }

                var grouponCondition = _grouponConditionRepository.GetFiltered(o => o.GoodsId == id).Select(item => new GrouponConditionDTO { Id = item.Id, GoodsId = item.GoodsId, MoreThanNumber = item.MoreThanNumber, Price = item.Price });

                return new GoodsDTO
                {
                    Category = obj.Category,
                    Description = obj.Description,
                    Detail = obj.Detail,
                    Id = obj.Id,
                    ItemNumber = obj.ItemNumber,
                    MarketPrice = obj.MarketPrice,
                    OptionalPropertyJsonObject = obj.OptionalPropertyJsonObject,
                    Status = obj.Status,
                    Stock = obj.Stock,
                    StoreId = obj.StoreId,
                    SubCategory = obj.SubCategory,
                    Title = obj.Title,
                    Unit = obj.Unit,
                    UnitPrice = obj.UnitPrice,
                    GoodsImages = goodsImages,
                    GrouponConditions = grouponCondition,
                    DistributionScope = obj.DistributionScope
                };
            }
            return null;
        }

        public async Task<GoodsDTO> GetOnlineGoods(string id)
        {
            var obj = _onlineGoodsRepository.Get(id);

            if (obj != null)
            {
                var goodsImages = _onlineGoodsImageRepository.GetFiltered(o => o.GoodsId == id).Select(item => new GoodsImageDTO { GoodsId = item.GoodsId, Id = item.Id, ImageId = item.ImageId }).ToList();

                var imageIds = goodsImages.Select(o => o.ImageId).ToList();
                var images = await _imageServiceProxy.GetImagesByIds(imageIds);
                foreach (var img in goodsImages)
                {
                    var pic = images.FirstOrDefault(o => o.Id == img.ImageId);

                    img.HttpPath = pic?.HttpPath;
                    img.Title = pic?.Title;
                    img.Description = pic?.Description;
                }

                var grouponCondition = _onlineGrouponConditionRepository.GetFiltered(o => o.GoodsId == id).Select(item => new GrouponConditionDTO { Id = item.Id, GoodsId = item.GoodsId, MoreThanNumber = item.MoreThanNumber, Price = item.Price });

                return new GoodsDTO
                {
                    Category = obj.Category,
                    Description = obj.Description,
                    Detail = obj.Detail,
                    Id = obj.Id,
                    ItemNumber = obj.ItemNumber,
                    MarketPrice = obj.MarketPrice,
                    OptionalPropertyJsonObject = obj.OptionalPropertyJsonObject,
                    Status = obj.Status,
                    Stock = obj.Stock,
                    StoreId = obj.StoreId,
                    SubCategory = obj.SubCategory,
                    Title = obj.Title,
                    Unit = obj.Unit,
                    UnitPrice = obj.UnitPrice,
                    GoodsImages = goodsImages,
                    GrouponConditions = grouponCondition,
                    DistributionScope = obj.DistributionScope
                };
            }
            return null;
        }

        public async Task<IEnumerable<GoodsDTO>> GetGoods(string key, string category, string subCategory, int pageIndex, int pageSize, string storeId = "")
        {
            IEnumerable<GoodsDTO> goods = null;

            var result = _goodsRepository.GetFiltered(o => (o.StoreId == storeId || string.IsNullOrEmpty(storeId)));

            if (!string.IsNullOrEmpty(key)) result = result.Where(o => o.Title.Contains(key));

            if (!string.IsNullOrEmpty(category)) result = result.Where(o => o.Category == category);

            if (!string.IsNullOrEmpty(subCategory)) result = result.Where(o => o.SubCategory == subCategory);

            goods = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(
                item => new GoodsDTO
                {
                    Category = item.Category,
                    Description = item.Description,
                    Detail = item.Detail,
                    Id = item.Id,
                    ItemNumber = item.ItemNumber,
                    MarketPrice = item.MarketPrice,
                    OptionalPropertyJsonObject = item.OptionalPropertyJsonObject,
                    Status = item.Status,
                    Stock = item.Stock,
                    StoreId = item.StoreId,
                    SubCategory = item.SubCategory,
                    Title = item.Title,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    DistributionScope = item.DistributionScope
                }).ToList();

            if (goods != null && goods.Count() > 0)
            {
                foreach (var item in goods)
                {
                    item.GoodsImages = _goodsImageRepository.GetFiltered(o => o.GoodsId == item.Id).Select(image => new GoodsImageDTO { ImageId = image.ImageId, GoodsId = image.GoodsId, Id = image.Id }).ToList();

                    var imageIds = item.GoodsImages.Select(o => o.ImageId).ToList();
                    var images = await _imageServiceProxy.GetImagesByIds(imageIds);
                    foreach (var img in item.GoodsImages)
                    {
                        var pic = images.FirstOrDefault(o => o.Id == img.ImageId);

                        img.HttpPath = pic?.HttpPath;
                        img.Title = pic?.Title;
                        img.Description = pic?.Description;
                    }
                }
            }
            return goods;
        }

        public async Task<IEnumerable<GoodsDTO>> GetOnlineGoods(string key, string category, string subCategory, int pageIndex, int pageSize)
        {
            IEnumerable<GoodsDTO> goods = null;

            var result = _onlineGoodsRepository.GetFiltered(o => o.Status == GoodsStatus.Shelved || o.Status == GoodsStatus.SoldOut || o.Status == GoodsStatus.UnShelved);

            if (!string.IsNullOrEmpty(key)) result = result.Where(o => o.Title.Contains(key));

            if (!string.IsNullOrEmpty(category)) result = result.Where(o => o.Category == category);

            if (!string.IsNullOrEmpty(subCategory)) result = result.Where(o => o.SubCategory == subCategory);

            goods = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(
                item => new GoodsDTO
                {
                    Category = item.Category,
                    Description = item.Description,
                    Detail = item.Detail,
                    Id = item.Id,
                    ItemNumber = item.ItemNumber,
                    MarketPrice = item.MarketPrice,
                    OptionalPropertyJsonObject = item.OptionalPropertyJsonObject,
                    Status = item.Status,
                    Stock = item.Stock,
                    StoreId = item.StoreId,
                    SubCategory = item.SubCategory,
                    Title = item.Title,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    DistributionScope = item.DistributionScope
                }).ToList();

            if (goods != null && goods.Count() > 0)
            {
                foreach (var item in goods)
                {
                    item.GoodsImages = _onlineGoodsImageRepository.GetFiltered(o => o.GoodsId == item.Id).Select(image => new GoodsImageDTO { ImageId = image.ImageId, GoodsId = image.GoodsId, Id = image.Id }).ToList();

                    var imageIds = item.GoodsImages.Select(o => o.ImageId).ToList();
                    var images = await _imageServiceProxy.GetImagesByIds(imageIds);
                    foreach (var img in item.GoodsImages)
                    {
                        var pic = images.FirstOrDefault(o => o.Id == img.ImageId);

                        img.HttpPath = pic?.HttpPath;
                        img.Title = pic?.Title;
                        img.Description = pic?.Description;
                    }
                }
            }
            return goods;
        }

        public async Task<IEnumerable<GoodsDTO>> GetNearbyGoods(double longitude, double latitude, string category, string subCategory, int pageIndex, int pageSize, string appId = "GOOIOS001")
        {
            var goods = new List<GoodsDTO>();

            var result = _onlineGoodsRepository.GetPaged(pageIndex, pageSize,
                o => (o.Status == GoodsStatus.Shelved || o.Status == GoodsStatus.SoldOut || o.Status == GoodsStatus.UnShelved)
                && (o.Category == category || string.IsNullOrEmpty(category))
                && (o.SubCategory == subCategory || string.IsNullOrEmpty(subCategory))
                && (string.IsNullOrEmpty(appId) || o.ApplicationId == appId)
                && (o.DistributionScope >= (GetDistance(longitude, latitude, o.Longitude, o.Latitude) / 1000) || o.DistributionScope == 0),
                o => GetDistance(longitude, latitude, o.Longitude, o.Latitude), true);

            foreach (var item in result)
            {
                var organization = await _organizationServiceProxy.GetOrganizationById(item.StoreId);
                var logoImgUrl = "";
                if (organization != null)
                {
                    var logoImg = await _imageServiceProxy.GetImageById(organization.LogoImageId);
                    logoImgUrl = logoImg?.HttpPath;
                }

                var conditions = _grouponConditionRepository.GetFiltered(o => o.GoodsId == item.Id);

                goods.Add(new GoodsDTO
                {
                    Category = item.Category,
                    Description = item.Description,
                    Detail = item.Detail,
                    Id = item.Id,
                    ItemNumber = item.ItemNumber,
                    MarketPrice = item.MarketPrice,
                    OptionalPropertyJsonObject = item.OptionalPropertyJsonObject,
                    Status = item.Status,
                    Stock = item.Stock,
                    StoreId = item.StoreId,
                    SubCategory = item.SubCategory,
                    Title = item.Title,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    Address = item.Address,
                    Distance = ProcessDistance(GetDistance(longitude, latitude, item.Longitude, item.Latitude)),
                    DistributionScope = item.DistributionScope,
                    ApplicationId = item.ApplicationId,
                    OrganizationId = organization?.Id,
                    OrganizationLogoUrl = logoImgUrl,
                    OrganizationName = organization?.ShortName,
                    GrouponConditions = conditions?.Select(obj => new GrouponConditionDTO
                    {
                        GoodsId = obj.GoodsId,
                        Id = obj.Id,
                        MoreThanNumber = obj.MoreThanNumber,
                        Price = obj.Price
                    }).OrderBy(g => g.MoreThanNumber).ToList()
                });
            }

            if (goods != null && goods.Count() > 0)
            {
                foreach (var item in goods)
                {
                    item.GoodsImages = _onlineGoodsImageRepository.GetFiltered(o => o.GoodsId == item.Id).Select(image => new GoodsImageDTO { ImageId = image.ImageId, GoodsId = image.GoodsId, Id = image.Id }).ToList();

                    var imageIds = item.GoodsImages.Select(o => o.ImageId).ToList();
                    var images = await _imageServiceProxy.GetImagesByIds(imageIds);
                    foreach (var img in item.GoodsImages)
                    {
                        var pic = images.FirstOrDefault(o => o.Id == img.ImageId);

                        img.HttpPath = pic?.HttpPath;
                        img.Title = pic?.Title;
                        img.Description = pic?.Description;
                    }
                }
            }
            return goods;
        }

        public void ShelveGoods(string id, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var goods = _goodsRepository.Get(id);
                if (goods != null)
                {
                    goods.GoodsImages = _goodsImageRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();
                    goods.GrouponConditions = _grouponConditionRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();
                    goods.ResolveAddress();
                    goods.SetShelved();
                    _goodsRepository.Update(goods);
                    goods.ShelvedConfirm();
                    coordinator.Commit();
                }
            }
        }

        public void SuspendGoods(string id, string operatorId)
        {
            var onlineGoods = _onlineGoodsRepository.Get(id);
            var goods = _goodsRepository.Get(id);
            if (onlineGoods != null)
            {
                onlineGoods.SetSuspend();
                goods.SetSuspend();
                _goodsRepository.Update(goods);
                _onlineGoodsRepository.Update(onlineGoods);
                _dbUnitOfWork.Commit();
            }
        }

        public void UnShelveGoods(string id, string operatorId)
        {
            var onlineGoods = _onlineGoodsRepository.Get(id);
            var goods = _goodsRepository.Get(id);
            if (onlineGoods != null)
            {
                onlineGoods.SetUnShelved();
                goods.SetUnShelved();
                _goodsRepository.Update(goods);
                _onlineGoodsRepository.Update(onlineGoods);
                _dbUnitOfWork.Commit();
            }
        }

        public async Task<string> ConfirmBuyGoods(ConfirmBuyGoodsDTO model, string operatorId)
        {
            string orderId = "";

            var user = await _authServiceProxy.GetUser(operatorId);
            if (user == null) throw new Exception("Account information exception.");

            var goods = _onlineGoodsRepository.Get(model.GoodsId);
            if (goods == null) throw new Exception("Goods information exception.");

            var conditions = _grouponConditionRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();
            var images = _onlineGoodsImageRepository.GetFiltered(o => o.GoodsId == goods.Id).ToList();

            var gimg = images?.FirstOrDefault();
            var previewImgUrl = "";
            if (gimg != null && !string.IsNullOrEmpty(gimg.ImageId))
            {
                var img = await _imageServiceProxy.GetImageById(gimg.ImageId);
                previewImgUrl = img?.HttpPath;
            }

            if (model.Mode == "grouponbuy")
            {
                var activity = await _activityServiceProxy.CreateGrouponActivity(new Proxies.DTOs.GrouponActivityDTO
                {
                    Count = conditions?.OrderBy(o => o.MoreThanNumber).FirstOrDefault()?.MoreThanNumber ?? 1,
                    CreatedBy = operatorId,
                    CreatedOn = DateTime.Now,
                    CreatorName = user.NickName,
                    CreatorPortraitUrl = user.PortraitUrl,
                    End = DateTime.Now.AddHours(24),
                    ProductId = model.GoodsId,
                    ProductMark = "Goods",
                    Start = DateTime.Now,
                    Status = Proxies.DTOs.ActivityStatus.InProgress,
                    UnitPrice = conditions?.OrderBy(o => o.MoreThanNumber).FirstOrDefault()?.Price ?? goods.UnitPrice
                });

                if (activity == null) throw new Exception("create groupon activity failed.");

                var goodsPrice = conditions?.OrderBy(o => o.MoreThanNumber).FirstOrDefault()?.Price ?? goods.UnitPrice;
                var rmk = "";

                foreach (var item in model.SelectedProperties)
                {
                    rmk += $"  {item.Name}:{item.Value}  ";
                }
                var order = await _orderServiceProxy.CreateOrder(new OrderDTO
                {
                    CreatedBy = operatorId,
                    CreatedOn = DateTime.Now,
                    CustomerAddress = model.CustomerAddress,
                    CustomerMobile = model.Mobile,
                    CustomerName = model.CustomerName,
                    Invoiceremark = "",
                    InvoiceType = Proxies.DTOs.InvoiceType.None,
                    Mark = "Goods",
                    OrganizationId = goods.StoreId,
                    PayAmount = goodsPrice,
                    PreferentialAmount = 0,
                    Remark = rmk,
                    ShippingCost = 0,
                    Tax = 0,
                    TotalAmount = goodsPrice,
                    UpdatedBy = operatorId,
                    UpdatedOn = DateTime.Now,
                    OrderItems = new List<OrderItemDTO> {
                        new OrderItemDTO{
                            Count=1,
                            ObjectId =goods.Id,
                            ObjectNo =goods.ItemNumber,
                            PreviewPictureUrl =previewImgUrl,
                            SelectedProperties =JsonConvert.SerializeObject(model.SelectedProperties),
                            Title =goods.Title,
                            TradeUnitPrice = goodsPrice
                        }
                    },
                    ActivityId = activity.Id
                });

                if (order == null) throw new Exception("create order failed.");

                //await _activityServiceProxy.AddGrouponParticipation(new Proxies.DTOs.GrouponParticipationDTO
                //{
                //    BuyCount = 1,
                //    GrouponActivityId = activity.Id,
                //    NickName = user.NickName,
                //    OrderId = order.Id,
                //    UserId = user.Id,
                //    UserPortraitUrl = user.PortraitUrl
                //});

                orderId = order?.Id ?? "";
            }

            if (model.Mode == "attendgroup")
            {
                var activity = await _activityServiceProxy.GetActivityById(model.ActivityId);

                if (activity == null) throw new Exception("活动Id异常.找不到指定的活动信息.");

                var goodsPrice = conditions?.OrderBy(o => o.MoreThanNumber).FirstOrDefault()?.Price ?? goods.UnitPrice;
                var rmk = "";

                foreach (var item in model.SelectedProperties)
                {
                    rmk += $"  {item.Name}:{item.Value}  ";
                }

                var order = await _orderServiceProxy.CreateOrder(new OrderDTO
                {
                    CreatedBy = operatorId,
                    CreatedOn = DateTime.Now,
                    CustomerAddress = model.CustomerAddress,
                    CustomerMobile = model.Mobile,
                    CustomerName = model.CustomerName,
                    Invoiceremark = "",
                    InvoiceType = Proxies.DTOs.InvoiceType.None,
                    Mark = "Goods",
                    OrganizationId = goods.StoreId,
                    PayAmount = goodsPrice,
                    PreferentialAmount = 0,
                    Remark = rmk,
                    ShippingCost = 0,
                    Tax = 0,
                    TotalAmount = goodsPrice,
                    UpdatedBy = operatorId,
                    UpdatedOn = DateTime.Now,
                    OrderItems = new List<OrderItemDTO> {
                        new OrderItemDTO{
                            Count=1,
                            ObjectId =goods.Id,
                            ObjectNo =goods.ItemNumber,
                            PreviewPictureUrl =previewImgUrl,
                            SelectedProperties =JsonConvert.SerializeObject(model.SelectedProperties),
                            Title =goods.Title,
                            TradeUnitPrice = goodsPrice
                        }
                    },
                    ActivityId = activity.Id
                });

                if (order == null) throw new Exception("create order failed.");

                //await _activityServiceProxy.AddGrouponParticipation(new GrouponParticipationDTO
                //{
                //    BuyCount = 1,
                //    GrouponActivityId = model.ActivityId,
                //    NickName = user.NickName,
                //    OrderId = order.Id,
                //    UserId = user.Id,
                //    UserPortraitUrl = user.PortraitUrl
                //});

                orderId = order.Id;
            }

            if (model.Mode == "buy")
            {

                var goodsPrice = goods.UnitPrice;
                var rmk = "";

                foreach (var item in model.SelectedProperties)
                {
                    rmk += $"  {item.Name}:{item.Value}  ";
                }
                var order = await _orderServiceProxy.CreateOrder(new OrderDTO
                {
                    CreatedBy = operatorId,
                    CreatedOn = DateTime.Now,
                    CustomerAddress = model.CustomerAddress,
                    CustomerMobile = model.Mobile,
                    CustomerName = model.CustomerName,
                    Invoiceremark = "",
                    InvoiceType = Proxies.DTOs.InvoiceType.None,
                    Mark = "Goods",
                    OrganizationId = goods.StoreId,
                    PayAmount = goodsPrice,
                    PreferentialAmount = 0,
                    Remark = rmk,
                    ShippingCost = 0,
                    Tax = 0,
                    TotalAmount = goodsPrice,
                    UpdatedBy = operatorId,
                    UpdatedOn = DateTime.Now,
                    OrderItems = new List<OrderItemDTO> {
                        new OrderItemDTO{
                            Count=1,
                            ObjectId =goods.Id,
                            ObjectNo =goods.ItemNumber,
                            PreviewPictureUrl =previewImgUrl,
                            SelectedProperties =JsonConvert.SerializeObject(model.SelectedProperties),
                            Title =goods.Title,
                            TradeUnitPrice = goodsPrice
                        }
                    }
                });

                if (order == null) throw new Exception("create order failed.");

                orderId = order.Id;
            }

            return orderId;
        }

        #region 根据经纬度计算距离
        const double EARTH_RADIUS = 6378137;
        double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }
        double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

        string ProcessDistance(double distance)
        {
            if (distance < 1000) return $"{String.Format("{0:N0} ", distance)}m";
            return $"{String.Format("{0:N2} ", distance / 1000)}km";
        }
        #endregion

    }

    public class TmpGenerate
    {
        public static void StaticCall()
        {
            var uow = IocProvider.GetService<IDbUnitOfWork>();
            var t = IocProvider.GetService<ICommentAppService>();
            Console.WriteLine("StaticCall uow ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow.GetHashCode());
            Console.WriteLine("StaticCall t ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + t.GetHashCode());
        }

        public void InstanceCall()
        {
            var uow = IocProvider.GetService<IDbUnitOfWork>();
            var t = IocProvider.GetService<ICommentAppService>();
            Console.WriteLine("InstanceCall uow ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow.GetHashCode());
            Console.WriteLine("InstanceCall t ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + t.GetHashCode());
        }
    }

    public class TmpInstanceGenerate
    {
        readonly IDbUnitOfWork _uow;
        readonly IDbUnitOfWork _uow2;
        public TmpInstanceGenerate(IDbUnitOfWork uow, IDbUnitOfWork uow2)
        {
            _uow = uow;
            _uow2 = uow2;
        }

        public void InstanceCall()
        {
            var uow = IocProvider.Container.Resolve<IDbUnitOfWork>();//IocProvider.GetService<IDbUnitOfWork>();
            Console.WriteLine("InstanceCall uow ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow.GetHashCode());
            var uow2 = IocProvider.Container.Resolve<IDbUnitOfWork>();
            Console.WriteLine("InstanceCall uow2 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow2.GetHashCode());

            using (var scope = IocProvider.Container.BeginLifetimeScope())
            {
                var uow5 = scope.Resolve<IDbUnitOfWork>();//IocProvider.GetService<IDbUnitOfWork>();
                Console.WriteLine("InstanceCall uow5 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow5.GetHashCode());
                var uow6 = scope.Resolve<IDbUnitOfWork>();
                Console.WriteLine("InstanceCall uow6 ---------->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + uow6.GetHashCode());
            }
        }
    }

}
