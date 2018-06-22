using Gooios.OrderService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public class OrderTrace:Entity<int>
    {
        public string OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}
