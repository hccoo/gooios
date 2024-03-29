﻿using Gooios.OrderService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Repositories
{
    public interface IDeliveryAddressRepository : IRepository<DeliveryAddress, string>
    {
    }
}
