using Gooios.AppletUserService.Domains.Aggregates;
using Gooios.AppletUserService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AppletUserService.Repositories
{
    public class UserSessionRepository : Repository<UserSession, int>, IUserSessionRepository
    {
        public UserSessionRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
