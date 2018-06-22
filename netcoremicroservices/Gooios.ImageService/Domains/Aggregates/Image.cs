using Gooios.ImageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ImageService.Domains.Aggregates
{
    public class Image : Entity<string>
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string HttpPath { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
