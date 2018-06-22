using Gooios.Infrastructure;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Applications.Services
{
    public interface IDeliveryNoteAppService : IApplicationServiceContract
    {
        void AddDeliveryNote(DeliveryNoteDTO model, string operatorId);

        IEnumerable<DeliveryNoteDTO> GetByOrderId(string orderId);
    }

    public class DeliveryNoteAppService : ApplicationServiceContract, IDeliveryNoteAppService
    {
        readonly IDeliveryNoteRepository _deliveryNoteRepository;
        readonly IOrderRepository _orderRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;

        public DeliveryNoteAppService(IDeliveryNoteRepository deliveryNoteRepository,IOrderRepository orderRepository, IDbUnitOfWork dbUnitOfWork)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
            _orderRepository = orderRepository;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void AddDeliveryNote(DeliveryNoteDTO model, string operatorId)
        {
            var order = _orderRepository.Get(model.OrderId);

            if (order == null) throw new Exception("找不到指定的订单.");

            var obj = DeliveryNoteFactory.CreateDeliveryNote(order, model.DeliveryNoteNo, model.Consignee, model.ConsigneeMobile, model.DeliveryAddress, model.ShippingMethod, model.CarrierName, model.CarrierPhone, model.ShippingAmount, operatorId);

            _deliveryNoteRepository.Add(obj);

            _dbUnitOfWork.Commit();
        }

        public IEnumerable<DeliveryNoteDTO> GetByOrderId(string orderId)
        {
            return _deliveryNoteRepository.GetFiltered(o => o.OrderId == orderId).Select(item => new DeliveryNoteDTO
            {
                ShippingMethod = item.ShippingMethod,
                ShippingAmount = item.ShippingAmount,
                OrderId = item.OrderId,
                CarrierName = item.CarrierName,
                CarrierPhone = item.CarrierPhone,
                Consignee = item.Consignee,
                ConsigneeMobile = item.ConsigneeMobile,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                DeliveryAddress = item.DeliveryAddress,
                DeliveryNoteNo = item.DeliveryNoteNo,
                Id = item.Id
            });
        }
    }
}
