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
    [Route("api/service/v1")]
    public class ServiceController : BaseApiController
    {
        readonly IServiceAppService _serviceAppService;
        public ServiceController(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }

        [HttpPost]
        public async Task Post([FromBody]ServiceDTO model)
        {
            await _serviceAppService.AddService(model, UserId);
        }

        [HttpPut]
        public async Task Put([FromBody]ServiceDTO model)
        {
            await _serviceAppService.UpdateService(model, UserId);
        }

        [HttpPut]
        [Route("suspend")]
        public void Suspend(string id)
        {
            _serviceAppService.SuspendService(id);
        }
        [HttpPut]
        [Route("delete")]
        public void Delete(string id)
        {
            _serviceAppService.DeleteService(id);
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<ServiceDTO> GetService(string id)
        {
            return await _serviceAppService.GetService(id);
        }

        [HttpGet]
        [Route("getnearbyservices")]
        public async Task<IEnumerable<ServiceDTO>> GetNearbyServices(double longitude, double latitude, int pageIndex, string key, string category, string subCategory, int pageSize = 20)
        {
            return await _serviceAppService.GetNearbyServices(longitude, latitude, pageIndex, pageSize, key, category, subCategory);
        }

        [HttpGet]
        [Route("getbyorganizationid")]
        public async Task<IEnumerable<ServiceDTO>> GetServices(string organizationId, int pageIndex, int pageSize = 20)
        {
            return await _serviceAppService.GetServices(organizationId, pageIndex, pageSize);
        }
    }
}