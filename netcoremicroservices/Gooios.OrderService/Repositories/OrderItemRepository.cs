using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Repositories
{
    public class OrderItemRepository : Repository<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
