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
    [Route("api/Tag")]
    public class TagController : BaseApiController
    {
        readonly ITagAppService _tagAppService;
        public TagController(ITagAppService tagAppService)
        {
            _tagAppService = tagAppService;
        }

        [HttpGet]
        public IEnumerable<TagDTO> GetByReservationId(string reservationId)
        {
            return _tagAppService.GetByReservationId(reservationId);
        }
    }
}