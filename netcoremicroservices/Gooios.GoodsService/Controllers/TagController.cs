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
    [Route("api/tag/v1")]
    public class TagController : BaseApiController
    {
        readonly ITagAppService _tagAppService;
        public TagController(ITagAppService tagAppService)
        {
            _tagAppService = tagAppService;
        }

        [HttpGet]
        public IEnumerable<TagDTO> GetByGoodsId(string goodsId)
        {
            return _tagAppService.GetByGoodsId(goodsId);
        }
    }
}