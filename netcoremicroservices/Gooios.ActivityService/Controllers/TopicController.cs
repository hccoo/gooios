using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.ActivityService.Applications.Services;
using Gooios.ActivityService.Applications.DTOs;

namespace Gooios.ActivityService.Controllers
{
    [Produces("application/json")]
    [Route("api/topic/v1")]
    public class TopicController : BaseApiController
    {
        readonly ITopicAppService _topicAppService;

        public TopicController(ITopicAppService topicAppService)
        {
            _topicAppService = topicAppService;
        }

        [HttpPost]
        public async Task<TopicDTO> Post([FromBody]TopicDTO model)
        {
            return await _topicAppService.AddTopic(model, UserId);
        }

        [HttpPut]
        public async Task Put([FromBody]TopicDTO model)
        {
            await _topicAppService.UpdateTopic(model, UserId);
        }

        [HttpGet]
        [Route("byorganizationid")]
        public async Task<IEnumerable<TopicDTO>> Get(int pageIndex, int pageSize, string organizationId, string key = "")
        {
            return await _topicAppService.GetTopicsByOrganizationId(pageIndex, pageSize, organizationId, key);
        }

        [HttpGet]
        [Route("nearby")]
        public async Task<IEnumerable<TopicDTO>> Get(double longitude, double latitude, int pageIndex, int pageSize, string key = "")
        {
            return await _topicAppService.GetNearbyTopics(longitude, latitude, pageIndex, pageSize, key);
        }

        [HttpGet]
        [Route("byid")]
        public async Task<TopicDTO> Get(string topicId, double? longitude = null, double? latitude = null)
        {
            return await _topicAppService.GetById(topicId, longitude, latitude);
        }

    }
}