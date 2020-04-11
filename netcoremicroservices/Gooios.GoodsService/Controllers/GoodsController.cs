using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.GoodsService.Applications.Services;
using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Proxies;
using Gooios.GoodsService.Domains.Aggregates;

namespace Gooios.GoodsService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/v1")]
    public class GoodsController : BaseApiController
    {
        readonly IGoodsAppService _goodsAppService;

        public GoodsController(IGoodsAppService goodsAppService)
        {
            _goodsAppService = goodsAppService;
        }

        [HttpPost]
        public void Post([FromBody]GoodsDTO model)
        {
            _goodsAppService.AddGoods(model, UserId);
        }

        [HttpPut]
        public void Put([FromBody]GoodsDTO model)
        {
            _goodsAppService.UpdateGoods(model, UserId);
        }

        [HttpPut]
        [Route("modifystock")]
        public void ModifyStock(string goodsId, int increment)
        {
            _goodsAppService.SetStock(goodsId, increment, UserId);
        }

        [HttpPut]
        [Route("applyforshelvegoods")]
        public void ApplyForShelveGoods([FromBody]GoodsIdDTO model)
        {
            _goodsAppService.ApplyForShelveGoods(model.Id, UserId);
        }

        [HttpPut]
        [Route("shelvegoods")]
        public void ShelveGoods([FromBody]GoodsIdDTO model)
        {
            _goodsAppService.ShelveGoods(model.Id, UserId);
        }
        [HttpPut]
        [Route("unshelvegoods")]
        public void UnShelveGoods([FromBody]GoodsIdDTO model)
        {
            _goodsAppService.UnShelveGoods(model.Id, UserId);
        }
        [HttpPut]
        [Route("suspendgoods")]
        public void SuspendGoods([FromBody]GoodsIdDTO model)
        {
            _goodsAppService.SuspendGoods(model.Id, UserId);
        }

        [HttpGet]
        [Route("getgoods")]
        public async Task<GoodsDTO> GetGoods(string id)
        {
            return await _goodsAppService.GetGoods(id);
        }
        [HttpGet]
        [Route("getonlinegoods")]
        public async Task<GoodsDTO> GetOnlineGoods(string id)
        {
            return await _goodsAppService.GetOnlineGoods(id);
        }
        [HttpGet]
        [Route("getpaginggoods")]
        public async Task<IEnumerable<GoodsDTO>> GetGoods(string key, string category, string subCategory, int pageIndex, int pageSize = 20,string storeId="")
        {
            return await _goodsAppService.GetGoods(key, category, subCategory, pageIndex, pageSize,storeId);
        }
        [HttpGet]
        [Route("getonlinepaginggoods")]
        public async Task<IEnumerable<GoodsDTO>> GetOnlineGoods(string key, string category, string subCategory, int pageIndex, int pageSize = 10)
        {
            return await _goodsAppService.GetOnlineGoods(key, category, subCategory, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("recommendgoods")]
        public async Task<IEnumerable<GoodsDTO>> GetRecommendGoods()
        {
            return await _goodsAppService.GetRecommendGoods();
        }

        [HttpGet]
        [Route("getnearbypaginggoods")]
        public async Task<IEnumerable<GoodsDTO>> GetNearbyGoods(double longitude, double latitude, string category, string subCategory, int pageIndex, int pageSize, string appId = "GOOIOS001")
        {
            return await _goodsAppService.GetNearbyGoods(longitude, latitude, category, subCategory, pageIndex, pageSize, appId);
        }

        [HttpPost]
        [Route("confirmbuygoods")]
        public async Task<string> ConfirmBuyGoods([FromBody]ConfirmBuyGoodsDTO model)
        {
            return await _goodsAppService.ConfirmBuyGoods(model, UserId);
        }

        [HttpGet]
        [Route("getgoodscategorynames")]
        public async Task<IEnumerable<string>> GetGoodsCategoryNames()
        {
            return await _goodsAppService.GetGoodsCategoryNames(UserId);
        }

        [HttpGet]
        [Route("getgoodsbygoodscategorynames")]
        public async Task<IEnumerable<GoodsDTO>> GetGoodsByGoodsCategoryName(string goodsCategoryName)
        {
            return await _goodsAppService.GetOnlineGoodsByGoodsCategoryName(goodsCategoryName);
        }
    }
}