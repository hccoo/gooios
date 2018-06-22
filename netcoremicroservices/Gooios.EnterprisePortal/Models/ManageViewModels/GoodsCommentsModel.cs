using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class GoodsCommentsModel
    {
        public string GoodsId { get; set; }

        public string OrderId { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> ImageIds { get; set; }

        public IEnumerable<string> TagIds { get; set; }
    }
}
