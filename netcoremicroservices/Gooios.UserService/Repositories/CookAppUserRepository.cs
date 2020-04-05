using Gooios.UserService.Domain.Aggregates;
using Gooios.UserService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Repositories
{
    public class CookAppUserRepository : Repository<CookAppUser, string>, ICookAppUserRepository
    {
        public CookAppUserRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
