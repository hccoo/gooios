using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class GoodsCategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public IEnumerable<GoodsCategoryTag> Tags { get; set; }

        public IEnumerable<GoodsCategoryModel> Children { get; set; }
    }

    public class GoodsCategoryTag
    {
        public string Name { get; set; }
    }
}
