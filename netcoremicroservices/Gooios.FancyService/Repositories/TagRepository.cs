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
    public class TagRepository : Repository<Tag, string>, ITagRepository
    {
        public TagRepository(IDbContextProvider provider) : base(provider)
        {

        }

        public IEnumerable<Tag> GetByReservationId(string reservationId)
        {
            var q = DataContext.Tags.FromSql($"select * from tags where category_id=(select id from categories where `name`=(select ifnull(sub_category,category) from services where id =(select service_id from reservations where id=@reservationId))) ", new MySqlParameter("reservationId", reservationId));
            return q.ToList();
        }
    }
}
