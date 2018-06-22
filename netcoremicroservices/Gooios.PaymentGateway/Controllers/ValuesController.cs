using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ICanPay.Core;
using ICanPay.Wechatpay;

namespace Gooios.PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        readonly IGateways _gateways;

        public ValuesController(IGateways gateways)
        {
            _gateways = gateways;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var order = new Order()
            {
                Amount = 0.01,
                OutTradeNo = "订单号",
                ProductId = "No124",
                Body = "夹克衫",
                Detail = "灰色夹克衫",
                OpenId = "kjjdlkfjsjfd",
                TradeType = "JSAPI"
            };
            var gateway = _gateways.Get<WechatpayGateway>(GatewayTradeType.Applet);
            gateway.Payment(order);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
