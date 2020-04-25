using Gooios.OrderService.Domains;
using Gooios.OrderService.Domains.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public class Order : Entity<string>
    {
        public string OrderNo { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal ShippingCost { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal PreferentialAmount { get; set; }

        /// <summary>
        /// 税费
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// 订单支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public OrderStatus Status { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public string InvoiceRemark { get; set; }

        public string Remark { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }

        public string DeliveryAddressId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 临时的，以后要通过消息机制传送
        /// </summary>
        public string ActivityId { get; set; } = null;

        [NotMapped]
        public IEnumerable<OrderItem> OrderItems { get; set; }

        [NotMapped]
        public Address CustomerAddress { get; set; }

        /// <summary>
        /// Service or Goods
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// Store id in goods service
        /// </summary>
        public string OrganizationId { get; set; }

        public void GenerateOrderNo()
        {
            OrderNo = OrderNoGenerator.GenerateOrderNo();
        }

        public void ResolveAddress()
        {
            if (CustomerAddress == null) CustomerAddress = new Address(Province, City, Area, StreetAddress, Postcode);
        }

        public void InitAddress()
        {
            Area = CustomerAddress.Area;
            City = CustomerAddress.City;
            Postcode = CustomerAddress.Postcode;
            Province = CustomerAddress.Province;
            StreetAddress = CustomerAddress.StreetAddress;
        }

        public void InitStatus()
        {
            if (this.PayAmount > 0)
            {
                Status = OrderStatus.Submitted;
            }
            else
            {
                Status = OrderStatus.Paid;
            }
        }

        public void SetPaid() => Status = OrderStatus.Paid;

        public void SetShipped() => Status = OrderStatus.Shipped;

        public void SetCancelled() => Status = OrderStatus.Cancelled;

        public void SetRefunded() => Status = OrderStatus.Refunded;

        public void SetCompleted() => Status = OrderStatus.Completed;

        public void SetFailed() => Status = OrderStatus.Failed;

        public void GenerateId() => Id = Guid.NewGuid().ToString();

        public bool IsSubmitStatus()
        {
            return Status == OrderStatus.Submitted;
        }

        public void ConfirmSubmited() => DomainEvent.Publish(new OrderSubmittedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, SubmittedTime = DateTime.Now });
        public void ConfirmPaid() => DomainEvent.Publish(new OrderPaidEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, PaidTime = DateTime.Now });
        public void ConfirmPaidFailed() => DomainEvent.Publish(new OrderPaidFailedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, FailedTime = DateTime.Now });
        public void ConfirmShipped() => DomainEvent.Publish(new OrderShippedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, ShippedTime = DateTime.Now });
        public void ConfirmRefund() => DomainEvent.Publish(new OrderRefundedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, RefundTime = DateTime.Now });
        public void ConfirmCompleted() => DomainEvent.Publish(new OrderCompletedEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, CompletedTime = DateTime.Now });
        public void ConfirmCancelled() => DomainEvent.Publish(new OrderCancelledEvent(this) { ID = Guid.NewGuid(), TimeStamp = DateTime.Now, CancelledTime = DateTime.Now });
    }

    public enum OrderStatus
    {
        Submitted = 1,
        Paid = 2,
        Shipped = 3,
        Cancelled = 4,
        Refunded = 5,
        Completed = 6,
        Failed = 7
    }

    public enum InvoiceType
    {
        VAT = 1,  //赠票
        Common = 2,//普票
        None = 3 //不开发票
    }
}
