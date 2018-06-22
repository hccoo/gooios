using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Repositories
{
    public interface ICommentRepository : IRepository<Comment, string>
    {
        IEnumerable<Comment> GetCommentsByServiceId(string serviceId, int pageIndex, int pageSize);

        IEnumerable<Comment> GetCommentsByServicerId(string servicerId, int pageIndex, int pageSize);
    }
}
