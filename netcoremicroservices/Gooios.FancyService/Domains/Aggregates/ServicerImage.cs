using Gooios.FancyService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class ServicerImage : Entity<int>
    {
        public string ServicerId { get; set; }

        public string ImageId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
