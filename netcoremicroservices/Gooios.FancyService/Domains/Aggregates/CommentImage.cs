using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class CommentImage : Entity<int>
    {
        public string CommentId { get; set; }

        public string ImageId { get; set; }
    }
}
