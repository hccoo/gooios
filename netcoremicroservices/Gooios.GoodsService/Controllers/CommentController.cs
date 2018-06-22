using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Applications.Services;

namespace Gooios.GoodsService.Controllers
{
    [Produces("application/json")]
    [Route("api/comment/v1")]
    public class CommentController : BaseApiController
    {
        readonly ICommentAppService _commentAppService;
        readonly IGoodsTagStatisticsAppService _goodsTagStatisticsAppService;

        public CommentController(ICommentAppService commentAppService,IGoodsTagStatisticsAppService goodsTagStatisticsAppService)
        {
            _commentAppService = commentAppService;
            _goodsTagStatisticsAppService = goodsTagStatisticsAppService;
        }

        [HttpPost]
        public void Post([FromBody]CommentDTO model)
        {
            _commentAppService.AddComment(model, UserId);
        }

        [HttpGet]
        public async Task<IEnumerable<CommentDTO>> Get(string goodsId, int pageIndex, int pageSize)
        {
            return await _commentAppService.Get(goodsId, pageIndex, pageSize);
        }
        
        [HttpGet]
        [Route("goodscommentsstatistics")]
        public async Task<GoodsTagStatisticsDTO> GetGoodsTagStatistics(string goodsId)
        {
            return _goodsTagStatisticsAppService.Get(goodsId);
        }

    }
}