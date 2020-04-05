using Gooios.UserService.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Domain.Repositories
{
    public interface ICookAppPartnerLoginUserRepository : IRepository<CookAppPartnerLoginUser, string>
    {
    }
}
