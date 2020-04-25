using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Applications.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gooios.OrderService.Controllers
{
    [Route("api/[controller]/v1")]
    public class DeliveryAddressController : BaseApiController
    {
        readonly IDeliveryAddressAppService _deliveryAddressAppService;

        public DeliveryAddressController(IDeliveryAddressAppService deliveryAddressAppService)
        {
            _deliveryAddressAppService = deliveryAddressAppService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<DeliveryAddressDTO> Get()
        {
            return _deliveryAddressAppService.GetMyDeliveryAddresses(UserId);
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]DeliveryAddressDTO value)
        {
            value.UserId = value.CreatedBy = UserId;            
            _deliveryAddressAppService.AddDeliveryAddress(value);
        }

    }
}
