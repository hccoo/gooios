using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.OrganizationService.Applications.Services;
using Gooios.OrganizationService.Applications.DTOs;

namespace Gooios.OrganizationService.Controllers
{
    [Produces("application/json")]
    [Route("api/organization/v1")]
    public class OrganizationController : BaseApiController
    {
        readonly IOrganizationAppService _organizationAppService;

        public OrganizationController(IOrganizationAppService organizationAppService)
        {
            _organizationAppService = organizationAppService;
        }

        [HttpPost]
        public void Post([FromBody]OrganizationDTO model)
        {
            _organizationAppService.AddOrganization(model, UserId);
        }

        [HttpPut]
        public void Put([FromBody]OrganizationDTO model)
        {
            _organizationAppService.UpdateOrganization(model, UserId);
        }

        [HttpGet]
        [Route("nearbyorganization")]
        public IEnumerable<OrganizationDTO> Get(double longitude, double latitude,int pageIndex)
        {
            return _organizationAppService.GetNearbyOrganizations(longitude, latitude, pageIndex, 10);
        }

        [HttpGet]
        [Route("getbyid")]
        public OrganizationDTO Get(string id)
        {
            return _organizationAppService.GetOrganizationById(id);
        }
    }
}