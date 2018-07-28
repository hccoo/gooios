using Gooios.AppletUserService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AppletUserService.Domains.Repositories
{
    public interface IUserSessionRepository : IRepository<UserSession, int>
    {
    }
}
