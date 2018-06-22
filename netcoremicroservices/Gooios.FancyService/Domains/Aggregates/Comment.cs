using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class Comment : Entity<string>
    {
        public string ReservationId { get; set; }

        public string OrderId { get; set; }

        public string Content { get; set; }

        [NotMapped]
        public IEnumerable<string> ImageIds { get; set; }

        [NotMapped]
        public IEnumerable<CommentTag> CommentTags { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
