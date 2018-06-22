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
    [Route("api/[Controller]/v1")]
    public class GrouponParticipationController : BaseApiController
    {
        readonly IGrouponParticipationAppService _grouponParticipationAppService;
        public GrouponParticipationController(IGrouponParticipationAppService grouponParticipationAppService)
        {
            _grouponParticipationAppService = grouponParticipationAppService;
        }

        [HttpPost]
        public void Post([FromBody]GrouponParticipationDTO model)
        {
            model.UserId = UserId;
            _grouponParticipationAppService.CreateGrouponParticipation(model);
        }

        [HttpGet]
        public async Task<IEnumerable<GrouponParticipationDTO>> Get(string activityId, int pageIndex, int pageSize)
        {
            return await _grouponParticipationAppService.Get(activityId, pageIndex, pageSize);
        }
    }
}