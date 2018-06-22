using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.OrderService.Applications.Services;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Applications.DTOs;

namespace Gooios.OrderService.Controllers
{
    [Produces("application/json")]
    [Route("api/deliverynote/v1")]
    public class DeliveryNoteController : BaseApiController
    {
        readonly IDeliveryNoteAppService _deliveryNoteAppService;

        public DeliveryNoteController(IDeliveryNoteAppService deliveryNoteAppService)
        {
            _deliveryNoteAppService = deliveryNoteAppService;
        }

        [HttpPost]
        public void Post([FromBody]DeliveryNoteDTO model)
        {
            _deliveryNoteAppService.AddDeliveryNote(model, UserId);
        }

        [HttpGet]
        public IEnumerable<DeliveryNoteDTO> Get(string orderId)
        {
            return _deliveryNoteAppService.GetByOrderId(orderId);
        }
    }
}