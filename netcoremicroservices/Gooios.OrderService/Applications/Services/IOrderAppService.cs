using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Transactions;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gooios.OrderService.Applications.Services
{
    public interface IOrderAppService : IApplicationServiceContract
    {
        OrderDTO SubmitOrder(OrderDTO model, string operatorId);

        void OrderPaidSuccess(string orderId, string operatorId);

        void OrderPaidFailed(string orderId, string operatorId);

        void OrderShipped(string orderId, string operatorId);

        void OrderCancelled(string orderId, string operatorId);

        void OrderRefunded(string orderId, string operatorId);

        void OrderComplete(string orderId, string operatorId);

        void UpdateOrder(OrderDTO model, string operatorId);

        OrderDTO Get(string orderId);

        OrderDTO GetByNo(string orderNo);

        IEnumerable<OrderDTO> Get(string userId, int pageIndex, int pageSize);

        IEnumerable<OrderDTO> Get(OrderStatus status, int pageIndex, int pageSize);

        IEnumerable<OrderDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize);
    }

    public class OrderAppService : ApplicationServiceContract, IOrderAppService
    {
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly IOrderRepository _orderRepository;
        readonly IOrderItemRepository _orderItemRepository;
        readonly IOrderTraceRepository _orderTraceRepository;
        readonly IDeliveryNoteRepository _deliveryNoteRepository;

        public OrderAppService(
            IDbUnitOfWork dbUnitOfWork,
            IEventBus eventBus,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IOrderTraceRepository orderTraceRepository,
            IDeliveryNoteRepository deliveryNoteRepository
            )
        {
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _orderTraceRepository = orderTraceRepository;
            _deliveryNoteRepository = deliveryNoteRepository;
        }

        public OrderDTO Get(string orderId)
        {
            var obj = _orderRepository.Get(orderId);
            if (obj == null) throw new Exception("找不到指定的订单.");

            obj.ResolveAddress();

            var result = new OrderDTO
            {
                CustomerAddress = obj.CustomerAddress,
                CustomerMobile = obj.CustomerMobile,
                CustomerName = obj.CustomerName,
                Id = obj.Id,
                InvoiceType = obj.InvoiceType,
                OrderNo = obj.OrderNo,
                PayAmount = obj.PayAmount,
                PreferentialAmount = obj.PreferentialAmount,
                ShippingCost = obj.ShippingCost,
                Status = obj.Status,
                Tax = obj.Tax,
                TotalAmount = obj.TotalAmount,
                CreatedBy = obj.CreatedBy,
                CreatedOn = obj.CreatedOn,
                UpdatedBy = obj.UpdatedBy,
                UpdatedOn = obj.UpdatedOn,
                OrganizationId = obj.OrganizationId,
                Mark = obj.Mark,
                InvoiceRemark = obj.InvoiceRemark,
                Remark = obj.Remark,
                ActivityId = obj.ActivityId
            };

            var orderItems = _orderItemRepository.GetFiltered(o => o.OrderId == orderId).ToList();

            result.OrderItems = orderItems.Select(item => new OrderItemDTO
            {
                Count = item.Count,
                Id = item.Id,
                ObjectId = item.ObjectId,
                ObjectNo = item.ObjectNo,
                OrderId = item.OrderId,
                PreviewPictureUrl = item.PreviewPictureUrl,
                SelectedProperties = item.SelectedProperties,
                Title = item.Title,
                TradeUnitPrice = item.TradeUnitPrice
            }).ToList();

            return result;
        }

        public IEnumerable<OrderDTO> Get(string userId, int pageIndex, int pageSize)
        {
            return _orderRepository.GetFiltered(o => o.CreatedBy == userId).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(obj =>
            {
                obj.ResolveAddress();
                var result = new OrderDTO
                {
                    CustomerAddress = obj.CustomerAddress,
                    CustomerMobile = obj.CustomerMobile,
                    CustomerName = obj.CustomerName,
                    Id = obj.Id,
                    InvoiceType = obj.InvoiceType,
                    OrderNo = obj.OrderNo,
                    PayAmount = obj.PayAmount,
                    PreferentialAmount = obj.PreferentialAmount,
                    ShippingCost = obj.ShippingCost,
                    Status = obj.Status,
                    Tax = obj.Tax,
                    TotalAmount = obj.TotalAmount,
                    CreatedBy = obj.CreatedBy,
                    CreatedOn = obj.CreatedOn,
                    UpdatedBy = obj.UpdatedBy,
                    UpdatedOn = obj.UpdatedOn,
                    OrganizationId = obj.OrganizationId,
                    Mark = obj.Mark,
                    InvoiceRemark = obj.InvoiceRemark,
                    Remark = obj.Remark,
                    ActivityId = obj.ActivityId
                };
                var orderItems = _orderItemRepository.GetFiltered(o => o.OrderId == obj.Id).ToList();
                result.OrderItems = orderItems.Select(item => new OrderItemDTO
                {
                    Count = item.Count,
                    Id = item.Id,
                    ObjectId = item.ObjectId,
                    ObjectNo = item.ObjectNo,
                    OrderId = item.OrderId,
                    PreviewPictureUrl = item.PreviewPictureUrl,
                    SelectedProperties = item.SelectedProperties,
                    Title = item.Title,
                    TradeUnitPrice = item.TradeUnitPrice
                }).ToList();

                return result;
            }).ToList();
        }

        public IEnumerable<OrderDTO> GetByOrganizationId(string organizationId, int pageIndex, int pageSize)
        {
            return _orderRepository.GetFiltered(o => o.OrganizationId == organizationId).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(obj =>
            {
                var orderItems = _orderItemRepository.GetFiltered(o => o.OrderId == obj.Id).ToList();
                obj.ResolveAddress();
                var result = new OrderDTO
                {
                    CustomerAddress = obj.CustomerAddress,
                    CustomerMobile = obj.CustomerMobile,
                    CustomerName = obj.CustomerName,
                    Id = obj.Id,
                    InvoiceType = obj.InvoiceType,
                    OrderNo = obj.OrderNo,
                    PayAmount = obj.PayAmount,
                    PreferentialAmount = obj.PreferentialAmount,
                    ShippingCost = obj.ShippingCost,
                    Status = obj.Status,
                    Tax = obj.Tax,
                    TotalAmount = obj.TotalAmount,
                    CreatedBy = obj.CreatedBy,
                    CreatedOn = obj.CreatedOn,
                    UpdatedBy = obj.UpdatedBy,
                    UpdatedOn = obj.UpdatedOn,
                    OrganizationId = obj.OrganizationId,
                    Mark = obj.Mark,
                    InvoiceRemark = obj.InvoiceRemark,
                    Remark = obj.Remark,
                    ActivityId = obj.ActivityId,
                    Title = string.IsNullOrEmpty(obj.Title) ? orderItems.FirstOrDefault()?.Title : ""
                };
                result.OrderItems = orderItems.Select(item => new OrderItemDTO
                {
                    Count = item.Count,
                    Id = item.Id,
                    ObjectId = item.ObjectId,
                    ObjectNo = item.ObjectNo,
                    OrderId = item.OrderId,
                    PreviewPictureUrl = item.PreviewPictureUrl,
                    SelectedProperties = item.SelectedProperties,
                    Title = item.Title,
                    TradeUnitPrice = item.TradeUnitPrice
                }).ToList();

                return result;
            }).ToList();
        }

        public IEnumerable<OrderDTO> Get(OrderStatus status, int pageIndex, int pageSize)
        {
            return _orderRepository.GetFiltered(o => o.Status == status).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(obj =>
            {
                obj.ResolveAddress();
                var result = new OrderDTO
                {
                    CustomerAddress = obj.CustomerAddress,
                    CustomerMobile = obj.CustomerMobile,
                    CustomerName = obj.CustomerName,
                    Id = obj.Id,
                    InvoiceType = obj.InvoiceType,
                    OrderNo = obj.OrderNo,
                    PayAmount = obj.PayAmount,
                    PreferentialAmount = obj.PreferentialAmount,
                    ShippingCost = obj.ShippingCost,
                    Status = obj.Status,
                    Tax = obj.Tax,
                    TotalAmount = obj.TotalAmount,
                    CreatedBy = obj.CreatedBy,
                    CreatedOn = obj.CreatedOn,
                    UpdatedBy = obj.UpdatedBy,
                    UpdatedOn = obj.UpdatedOn,
                    OrganizationId = obj.OrganizationId,
                    Mark = obj.Mark,
                    InvoiceRemark = obj.InvoiceRemark,
                    Remark = obj.Remark,
                    ActivityId = obj.ActivityId
                };

                var orderItems = _orderItemRepository.GetFiltered(o => o.OrderId == obj.Id).ToList();
                result.OrderItems = orderItems.Select(item => new OrderItemDTO
                {
                    Count = item.Count,
                    Id = item.Id,
                    ObjectId = item.ObjectId,
                    ObjectNo = item.ObjectNo,
                    OrderId = item.OrderId,
                    PreviewPictureUrl = item.PreviewPictureUrl,
                    SelectedProperties = item.SelectedProperties,
                    Title = item.Title,
                    TradeUnitPrice = item.TradeUnitPrice
                }).ToList();

                return result;
            }).ToList();
        }

        public OrderDTO GetByNo(string orderNo)
        {
            var obj = _orderRepository.GetFiltered(o => o.OrderNo == orderNo).FirstOrDefault();
            if (obj == null) throw new Exception("找不到指定的订单.");

            obj.ResolveAddress();

            var result = new OrderDTO
            {
                CustomerAddress = obj.CustomerAddress,
                CustomerMobile = obj.CustomerMobile,
                CustomerName = obj.CustomerName,
                Id = obj.Id,
                InvoiceType = obj.InvoiceType,
                OrderNo = obj.OrderNo,
                PayAmount = obj.PayAmount,
                PreferentialAmount = obj.PreferentialAmount,
                ShippingCost = obj.ShippingCost,
                Status = obj.Status,
                Tax = obj.Tax,
                TotalAmount = obj.TotalAmount,
                CreatedBy = obj.CreatedBy,
                CreatedOn = obj.CreatedOn,
                UpdatedBy = obj.UpdatedBy,
                UpdatedOn = obj.UpdatedOn,
                OrganizationId = obj.OrganizationId,
                Mark = obj.Mark,
                InvoiceRemark = obj.InvoiceRemark,
                Remark = obj.Remark,
                ActivityId = obj.ActivityId
            };

            var orderItems = _orderItemRepository.GetFiltered(o => o.OrderId == obj.Id).ToList();

            result.OrderItems = orderItems.Select(item => new OrderItemDTO
            {
                Count = item.Count,
                Id = item.Id,
                ObjectId = item.ObjectId,
                ObjectNo = item.ObjectNo,
                OrderId = item.OrderId,
                PreviewPictureUrl = item.PreviewPictureUrl,
                SelectedProperties = item.SelectedProperties,
                Title = item.Title,
                TradeUnitPrice = item.TradeUnitPrice
            }).ToList();

            return result;
        }

        public void OrderCancelled(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetCancelled();
                order.ConfirmCancelled();

                coordinator.Commit();
            }
        }

        public void OrderComplete(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetCompleted();
                order.ConfirmCompleted();

                coordinator.Commit();
            }
        }

        public void OrderPaidFailed(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetFailed();
                order.ConfirmPaidFailed();

                coordinator.Commit();
            }
        }

        public void OrderPaidSuccess(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetPaid();

                order.ConfirmPaid();

                _orderRepository.Update(order);

                coordinator.Commit();
            }
        }

        public void OrderRefunded(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetRefunded();
                order.ConfirmRefund();

                coordinator.Commit();
            }
        }

        public void OrderShipped(string orderId, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var order = _orderRepository.Get(orderId);

                if (order == null) throw new Exception("找不到指定的订单.");

                order.SetShipped();
                order.ConfirmShipped();

                coordinator.Commit();
            }
        }

        public OrderDTO SubmitOrder(OrderDTO model, string operatorId)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var orderItems = model.OrderItems?.Select(item => new OrderItem { Count = item.Count, ObjectId = item.ObjectId, ObjectNo = item.ObjectNo, OrderId = item.OrderId, PreviewPictureUrl = item.PreviewPictureUrl, SelectedProperties = item.SelectedProperties, Title = item.Title, TradeUnitPrice = item.TradeUnitPrice })?.ToList();

                var obj = OrderFactory.CreateOrder(
                    model.TotalAmount,
                    model.ShippingCost,
                    model.PreferentialAmount,
                    model.Tax, model.PayAmount,
                    model.CustomerName,
                    model.CustomerMobile,
                    model.InvoiceType,
                    model.CustomerAddress,
                    orderItems,
                    operatorId,
                    model.Mark,
                    model.OrganizationId,
                    model.InvoiceRemark,
                    model.Remark,
                    model.ActivityId,
                    model.Title);

                _orderRepository.Add(obj);

                orderItems.ForEach(oi =>
                {
                    oi.OrderId = obj.Id;
                    _orderItemRepository.Add(oi);
                });

                obj.ConfirmSubmited();

                coordinator.Commit();

                model.Id = obj.Id;

                return model;
            }
        }

        public void UpdateOrder(OrderDTO model, string operatorId)
        {
            var obj = _orderRepository.Get(model.Id);
            if (obj == null) throw new Exception("找不到指定的订单.");
            if (!obj.IsSubmitStatus()) throw new Exception("订单无法修改.");

            obj.UpdatedBy = operatorId;
            obj.UpdatedOn = DateTime.Now;
            obj.Tax = model.Tax;
            obj.ShippingCost = model.ShippingCost;
            obj.PreferentialAmount = model.PreferentialAmount;
            obj.PayAmount = model.PayAmount;
            obj.InvoiceType = model.InvoiceType;
            obj.CustomerName = model.CustomerName;
            obj.CustomerMobile = model.CustomerMobile;
            obj.CustomerAddress = model.CustomerAddress;
            obj.Province = model.CustomerAddress?.Province ?? obj.Province;
            obj.City = model.CustomerAddress?.City ?? obj.City;
            obj.Area = model.CustomerAddress?.Area ?? obj.Area;
            obj.StreetAddress = model.CustomerAddress?.StreetAddress ?? obj.StreetAddress;
            obj.Postcode = model.CustomerAddress?.Postcode ?? obj.Postcode;
            obj.InvoiceRemark = model.InvoiceRemark;
            obj.Remark = model.Remark;
            obj.Title = model.Title;

            _orderRepository.Update(obj);
            _dbUnitOfWork.Commit();
        }
    }
}
