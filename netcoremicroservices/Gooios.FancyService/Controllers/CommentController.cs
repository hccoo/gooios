using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Applications.Services;

namespace Gooios.FancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/comment/v1")]
    public class CommentController : BaseApiController
    {
        readonly ICommentAppService _commentAppService;
        readonly ITagStatisticsAppService _tagStatisticsAppService;

        public CommentController(ICommentAppService commentAppService, ITagStatisticsAppService tagStatisticsAppService)
        {
            _commentAppService = commentAppService;
            _tagStatisticsAppService = tagStatisticsAppService;
        }
        [HttpPost]
        public void Post([FromBody]CommentDTO comment)
        {
            _commentAppService.AddComment(comment, UserId);
        }
        [HttpGet]
        [Route("getbyserviceid")]
        public async Task<IEnumerable<CommentDTO>> GetByServiceId(string serviceId, int pageIndex,int pageSize=20)
        {
            return await _commentAppService.GetByServiceId(serviceId, pageIndex, pageSize);
        }
        [HttpGet]
        [Route("getbyservicerid")]
        public async Task<IEnumerable<CommentDTO>> GetByServicerId(string servicerId, int pageIndex, int pageSize = 20)
        {
            return await _commentAppService.GetByServicerId(servicerId, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("servicecommentsstatistics")]
        public async Task<ServiceTagStatisticsDTO> GetServiceTagStatistics(string serviceId)
        {
            return _tagStatisticsAppService.GetServiceTagStatistics(serviceId);
        }

        [HttpGet]
        [Route("servicercommentsstatistics")]
        public async Task<ServicerTagStatisticsDTO> GetServicerTagStatistics(string servicerId)
        {
            return _tagStatisticsAppService.GetServicerTagStatistics(servicerId);
        }
    }
}