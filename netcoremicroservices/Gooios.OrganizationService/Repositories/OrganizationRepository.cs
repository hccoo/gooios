using Gooios.GoodsService.Repositories;
using Gooios.OrganizationService.Domains.Aggregates;
using Gooios.OrganizationService.Domains.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Repositories
{
    public class OrganizationRepository : Repository<Organization, string>, IOrganizationRepository
    {
        public OrganizationRepository(IDbContextProvider provider) : base(provider)
        {

        }
    }
}
