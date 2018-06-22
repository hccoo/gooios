using Gooios.Infrastructure.Events;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Events;
using Gooios.OrderService.Proxies;
using Gooios.OrderService.Proxies.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Events
{
    [HandlesAsynchronously]
    public class OrderPaidHandler : IEventHandler<OrderPaidEvent>
    {
        readonly IActivityServiceProxy _activityServiceProxy;
        readonly IGoodsServiceProxy _goodsServiceProxy;
        readonly IAuthServiceProxy _authServiceProxy;
        public OrderPaidHandler()
        {
            _activityServiceProxy = IocProvider.GetService<IActivityServiceProxy>();
            _goodsServiceProxy = IocProvider.GetService<IGoodsServiceProxy>();
            _authServiceProxy = IocProvider.GetService<IAuthServiceProxy>();
        }

        public void Handle(OrderPaidEvent evnt)
        {
            var order = evnt.Source as Order;

            if (order?.Mark == "Goods")
            {
                var user = _authServiceProxy.GetUser(order.CreatedBy).Result;

                if (!string.IsNullOrEmpty(order.ActivityId))
                {
                    _activityServiceProxy.AddGrouponParticipation(new Proxies.DTOs.GrouponParticipationDTO
                    {
                        BuyCount = 1,
                        GrouponActivityId = order.ActivityId,
                        NickName = user?.NickName,
                        OrderId = order.Id,
                        UserId = user?.Id,
                        UserPortraitUrl = user?.PortraitUrl
                    }).Wait();
                }
            }
            //TODO: Notificate the user

        }
    }
}
