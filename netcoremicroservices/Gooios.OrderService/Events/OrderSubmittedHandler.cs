using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Logs;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Events;
using Gooios.OrderService.Proxies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderSubmittedHandler : IEventHandler<OrderSubmittedEvent>
    {
        readonly IGoodsServiceProxy _goodsServiceProxy;
        readonly IFancyServiceProxy _fancyServiceProxy;
        public OrderSubmittedHandler()
        {
            _goodsServiceProxy = IocProvider.GetService<IGoodsServiceProxy>();
            _fancyServiceProxy = IocProvider.GetService<IFancyServiceProxy>();
        }

        public void Handle(OrderSubmittedEvent evnt)
        {
            LogProvider.Trace(new Log
            {
                ApplicationKey = "77e960be918111e709189226c7e9f002",
                AppServiceName = "OrderService",
                BizData = JsonConvert.SerializeObject(evnt),
                CallerApplicationKey = "",
                Exception = "",
                Level = LogLevel.INFO,
                LogThread = -1,
                LogTime = DateTime.Now,
                Operation = "OrderSubmittedHandler",
                ReturnValue = ""
            });
            var order = evnt.Source as Order;

            if (order != null)
            {
                if (order.Mark == "Goods")
                {
                    foreach (var item in order.OrderItems)
                    {
                        _goodsServiceProxy.SetStock(item.ObjectId, 0 - item.Count);
                    }
                }

                if (order.Mark == "Reservation")
                {
                    foreach (var item in order.OrderItems)
                    {
                        LogProvider.Trace(new Log
                        {
                            ApplicationKey = "77e960be918111e709189226c7e9f002",
                            AppServiceName = "OrderService",
                            BizData = JsonConvert.SerializeObject(item),
                            CallerApplicationKey = "",
                            Exception = "",
                            Level = LogLevel.INFO,
                            LogThread = -1,
                            LogTime = DateTime.Now,
                            Operation = "OrderSubmittedHandler",
                            ReturnValue = ""
                        });
                        _fancyServiceProxy.SetOrderId(new Proxies.DTOs.ReservationDTO
                        {
                            Id = item.ObjectId,
                            OrderId = order.Id
                        });
                    }
                }
            }
        }
    }
}
