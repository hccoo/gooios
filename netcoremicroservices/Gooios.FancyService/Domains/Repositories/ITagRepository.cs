using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Repositories
{
    public interface ITagRepository : IRepository<Tag, string>
    {
        IEnumerable<Tag> GetByReservationId(string reservationId);
    }
}
