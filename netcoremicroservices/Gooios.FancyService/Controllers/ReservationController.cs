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
    [Route("api/reservation/v1")]
    public class ReservationController : BaseApiController
    {
        readonly IReservationAppService _reservationAppService;
        public ReservationController(IReservationAppService reservationAppService)
        {
            _reservationAppService = reservationAppService;
        }

        [HttpPost]
        public async Task<ReservationDTO> Post([FromBody]ReservationDTO model)
        {
            return await _reservationAppService.AddReservation(model, UserId);
        }

        [HttpPatch]
        [Route("setunderway")]
        public void SetUnderway(string id)
        {
            _reservationAppService.SetReservationUnderway(id);
        }
        [HttpPatch]
        [Route("setcompleted")]
        public void SetCompleted(string id)
        {
            _reservationAppService.SetReservationCompleted(id);
        }
        [HttpPatch]
        [Route("setfailed")]
        public void SetFailed(string id)
        {
            _reservationAppService.SetReservationFailed(id);
        }
        [HttpPatch]
        [Route("setcancelled")]
        public void SetCancelled(string id)
        {
            _reservationAppService.SetReservationCancelled(id);
        }

        [HttpPut]
        [Route("setorderid")]
        public void SetOrderId([FromBody]ReservationDTO model)
        {
            _reservationAppService.SetOrderId(model);
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<ReservationDTO> Get(string id)
        {
            return await _reservationAppService.GetReservation(id);
        }

        [HttpGet]
        [Route("getbyuserid")]
        public IEnumerable<ReservationDTO> GetByUserId(string userId, int pageIndex, int pageSize = 20)
        {
            return _reservationAppService.GetByUserId(userId, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("getbyorganizationid")]
        public IEnumerable<ReservationDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize = 20)
        {
            return _reservationAppService.GetByOrganizationId(organizationId, pageIndex, pageSize);
        }
        [HttpGet]
        [Route("getmyreservations")]
        public IEnumerable<ReservationDTO> GetMyReservation(int pageIndex, int pageSize = 20)
        {
            return _reservationAppService.GetByUserId(UserId, pageIndex, pageSize);
        }
        
    }
}