using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Repositories
{
    public class CommentRepository : Repository<Comment, string>, ICommentRepository
    {
        public CommentRepository(IDbContextProvider provider) : base(provider)
        {

        }

        public IEnumerable<Comment> GetCommentsByServiceId(string serviceId, int pageIndex, int pageSize)
        {
            var q = DataContext.Comments.FromSql($"select c.* from comments c left join reservations r on c.reservation_id=r.id where r.service_id = @serviceId", new MySqlParameter("serviceId", serviceId));
            return q.OrderByDescending(o => o.CreatedOn).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<Comment> GetCommentsByServicerId(string servicerId, int pageIndex, int pageSize)
        {
            var q = DataContext.Comments.FromSql($"select c.* from comments c left join reservations r on c.reservation_id=r.id where r.servicer_id = @servicerId", new MySqlParameter("servicerId", servicerId));
            return q.OrderByDescending(o => o.CreatedOn).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
