using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Events;
using Gooios.FancyService.Proxies;
using Gooios.FancyService.Proxies.DTOs;
using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Events
{
    [HandlesAsynchronously]
    public class CreateOrderHandler : IEventHandler<AppointmentConfirmedEvent>
    {
        readonly IOrderServiceProxy _orderServiceProxy;
        public CreateOrderHandler()
        {
            _orderServiceProxy = IocProvider.GetService<IOrderServiceProxy>();
        }

        public void Handle(AppointmentConfirmedEvent evnt)
        {
            //var source = evnt.Source as Reservation;

            //var orderItems = new List<OrderItemDTO>();

            //orderItems.Add(new OrderItemDTO
            //{
            //    Count = 1,
            //    ObjectId = source.Id,
            //    ObjectNo = source.ReservationNo,
            //    Title = source.Service.Title,
            //    TradeUnitPrice = source.SincerityGoldNeedToPay,
            //    SelectedProperties = string.Empty,
            //    PreviewPictureUrl = string.Empty
            //});

            //_orderServiceProxy.CreateOrder(new Proxies.DTOs.OrderDTO
            //{
            //    CreatedBy = source.CreatedBy,
            //    CustomerAddress = source.ServiceDestination,
            //    CustomerMobile = source.CustomerMobile,
            //    CustomerName = source.CustomerName,
            //    InvoiceType = InvoiceType.None,
            //    PayAmount = orderItems.Sum(o => o.TradeUnitPrice * o.Count),
            //    PreferentialAmount = 0,
            //    ShippingCost = 0,
            //    Tax = 0,
            //    TotalAmount = source.SincerityGoldNeedToPay,
            //    OrderItems = orderItems,
            //    Mark = "Reservation",
            //    Invoiceremark = "",
            //    Remark = $"预约时间：{source?.AppointTime},预约服务：{source?.Service?.Title},服务者：{source?.Servicer?.Name??"未指定"}",
            //    OrganizationId = source.OrganizationId
            //});
        }
    }
}
