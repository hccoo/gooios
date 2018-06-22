using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.ActivityService.Applications.DTOs;
using Gooios.ActivityService.Applications.Services;

namespace Gooios.ActivityService.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]/v1")]
    public class GrouponActivityController : BaseApiController
    {
        readonly IGrouponActivityAppService _grouponActivityAppService;

        public GrouponActivityController(IGrouponActivityAppService grouponActivityAppService)
        {
            _grouponActivityAppService = grouponActivityAppService;
        }

        [HttpPost]
        public GrouponActivityDTO Post([FromBody]GrouponActivityDTO model)
        {
            return _grouponActivityAppService.CreateGrouponActivity(model, UserId);
        }

        [HttpGet]
        public async Task<IEnumerable<GrouponActivityDTO>> Get(string productId, string productMark)
        {
            return await _grouponActivityAppService.Get(productId, productMark);
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<GrouponActivityDTO> Get(string id)
        {
            return await _grouponActivityAppService.Get(id);
        }
    }
}