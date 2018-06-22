using Gooios.FancyService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class CommentTag:Entity<int>
    {
        public string CommentId { get; set; }

        public string TagId { get; set; }

        public string ReservationId { get; set; }

        public string UserId { get; set; }
    }
}
