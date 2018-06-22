using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.DTOs
{
    /// <summary>
    /// 服务跟服务人员的Category是一致的使用同一套
    /// </summary>
    public class CategoryDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public string Mark { get; set; }

        public string AppId { get; set; }

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
