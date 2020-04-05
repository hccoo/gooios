using Gooios.UserService.Domain.Aggregates;
using Gooios.UserService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Repositories
{
    public class CookAppPartnerLoginUserRepository : Repository<CookAppPartnerLoginUser, string>, ICookAppPartnerLoginUserRepository
    {
        public CookAppPartnerLoginUserRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
