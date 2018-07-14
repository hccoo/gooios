using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.FancyService.Applications.Services;
using Gooios.FancyService.Applications.DTOs;

namespace Gooios.FancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/servicer/v1")]
    public class ServicerController : BaseApiController
    {
        readonly IServicerAppService _servicerAppService;
        public ServicerController(IServicerAppService servicerAppService)
        {
            _servicerAppService = servicerAppService;
        }

        [HttpPost]
        public void Post([FromBody]ServicerDTO model)
        {
            _servicerAppService.AddServicer(model, UserId);
        }
        [HttpPut]
        public void Put([FromBody]ServicerDTO model)
        {
            _servicerAppService.UpdateServicer(model, UserId);
        }

        [HttpPut]
        [Route("suspend")]
        public void Suspend(string id)
        {
            _servicerAppService.SuspendServicer(id);
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<ServicerDTO> GetServicer(string id)
        {
            return await _servicerAppService.GetServicer(id);
        }

        [HttpGet]
        [Route("getbyorganizationid")]
        public async Task<IEnumerable<ServicerDTO>> GetServicers(string organizationId, int pageIndex, int pageSize = 20)
        {
            return await _servicerAppService.GetServicers(organizationId, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("getnearbyservicers")]
        public async Task<IEnumerable<ServicerDTO>> GetNearbyServicers(double longitude, double latitude, int pageIndex, string key, string category, string subCategory, int pageSize = 20, string appId = "")
        {
            return await _servicerAppService.GetNearbyServicers(longitude, latitude, pageIndex, pageSize, key, category, subCategory, appId);
        }
    }
}