using Gooios.OrderService.Repositories;
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Repositories
{
    public class DeliveryNoteRepository : Repository<DeliveryNote, string>, IDeliveryNoteRepository
    {
        public DeliveryNoteRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
