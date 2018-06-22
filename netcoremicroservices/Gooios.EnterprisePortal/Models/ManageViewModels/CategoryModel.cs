using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class CategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public string Mark { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<CategoryModel> Children { get; set; }
    }

    public class Tag
    {
        public string Name { get; set; }
    }
}
