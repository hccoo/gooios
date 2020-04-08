using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.OrderService.Applications.Services;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Domains.Aggregates;

namespace Gooios.OrderService.Controllers
{
    [Produces("application/json")]
    [Route("api/order/v1")]
    public class OrderController : BaseApiController
    {
        readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        [HttpPost]
        public OrderDTO Post([FromBody]OrderDTO model)
        {
            var operatorId = "";
            if (string.IsNullOrEmpty(UserId)) operatorId = model.CreatedBy;
            else operatorId = UserId;

            return _orderAppService.SubmitOrder(model, operatorId);
        }

        [HttpPut]
        [Route("modify")]
        public void Modify([FromBody]OrderDTO model)
        {
            _orderAppService.UpdateOrder(model, UserId);
        }

        [HttpPut]
        [Route("shipped")]
        public void SetShipped(string orderId)
        {
            _orderAppService.OrderShipped(orderId, UserId);
        }
        
        [HttpPut]
        [Route("refund")]
        public void SetRefunded(string orderId)
        {
            _orderAppService.OrderRefunded(orderId, UserId);
        }

        [HttpPut]
        [Route("paidsuccess")]
        public void SetPaidSuccess(string orderId)
        {
            _orderAppService.OrderPaidSuccess(orderId, UserId);
        }

        [HttpPut]
        [Route("paidfailed")]
        public void SetPaidFailed(string orderId)
        {
            _orderAppService.OrderPaidFailed(orderId, UserId);
        }

        [HttpPut]
        [Route("paidcomplete")]
        public void SetComplete(string orderId)
        {
            _orderAppService.OrderComplete(orderId, UserId);
        }

        [HttpPut]
        [Route("cancel")]
        public void SetCancelled(string orderId)
        {
            _orderAppService.OrderCancelled(orderId, UserId);
        }

        [HttpGet]
        [Route("orderno/{orderNo}")]
        public OrderDTO GetByOrderNo(string orderNo)
        {
            return _orderAppService.GetByNo(orderNo);
        }

        [HttpGet]
        [Route("orderid/{orderId}")]
        public OrderDTO GetById(string orderId)
        {
            return _orderAppService.Get(orderId);
        }

        [HttpGet]
        [Route("byuserid")]
        public IEnumerable<OrderDTO> Get(string userId, int pageIndex, int pageSize)
        {
            userId = UserId;
            return _orderAppService.Get(userId, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("userid")]
        public IEnumerable<OrderDTO> Get(int pageIndex, int pageSize)
        {
            return _orderAppService.Get(UserId, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("bystatus")]
        public IEnumerable<OrderDTO> Get(OrderStatus status, int pageIndex, int pageSize = 20)
        {
            return _orderAppService.Get(status, pageIndex, pageSize);
        }

        [HttpGet]
        [Route("byorganizationid")]
        public IEnumerable<OrderDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize = 20)
        {
            return _orderAppService.GetByOrganizationId(organizationId, pageIndex, pageSize);
        }

    }
}