using Gooios.OrganizationService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Domains.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization, string>
    {
    }
}
