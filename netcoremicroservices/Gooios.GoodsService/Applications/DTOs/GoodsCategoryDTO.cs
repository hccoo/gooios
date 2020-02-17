using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class GoodsCategoryDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }


        public string ParentId { get; set; }

        public int Order { get; set; }

        public IEnumerable<TagDTO> Tags { get; set; }
    }

    public class TagDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CategoryId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }

}
