using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public class GoodsCategory : Entity<string>
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string ParentId { get; set; }

        public string ApplicationId { get; set; }

        public int Order { get; set; }

        [NotMapped]
        public IEnumerable<Tag> Tags { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
