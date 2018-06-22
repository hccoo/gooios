using Gooios.FancyService.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public class Category : Entity<string>
    {
        public string Name { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// Service与Servicer
        /// </summary>
        public string Mark { get; set; }

        public string ApplicationId { get; set; } = "GOOIOS001";

        public int Order { get; set; }

        [NotMapped]
        public IEnumerable<Tag> Tags { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
