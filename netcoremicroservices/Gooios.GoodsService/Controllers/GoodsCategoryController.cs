using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.GoodsService.Applications.Services;
using Gooios.GoodsService.Applications.DTOs;

namespace Gooios.GoodsService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/v1")]
    public class GoodsCategoryController : BaseApiController
    {
        readonly IGoodsCategoryAppService _goodsCategoryAppService;

        public GoodsCategoryController(IGoodsCategoryAppService goodsCategoryAppService)
        {
            _goodsCategoryAppService = goodsCategoryAppService;
        }

        [HttpPost]
        public void Post([FromBody]GoodsCategoryDTO model)
        {
            //var userId = Request.Headers["userId"].FirstOrDefault();
            _goodsCategoryAppService.AddGoodsCategory(model, UserId);
        }

        [HttpGet]
        public IEnumerable<GoodsCategoryDTO> Get(string appId = "")
        {
            return _goodsCategoryAppService.GetAllGoodsCategories(appId);
        }
    }
}