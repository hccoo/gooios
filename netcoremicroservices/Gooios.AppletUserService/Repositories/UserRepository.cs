using Gooios.AppletUserService.Domains.Aggregates;
using Gooios.AppletUserService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AppletUserService.Repositories
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
