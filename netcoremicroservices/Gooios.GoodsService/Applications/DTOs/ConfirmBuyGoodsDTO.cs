using Gooios.GoodsService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class ConfirmBuyGoodsDTO
    {
        public string GoodsId { get; set; }

        public string ActivityId { get; set; }

        public Address CustomerAddress { get; set; }

        public string CustomerName { get; set; }

        public string Mobile { get; set; }

        /// <summary>
        /// grouponbuy,attendgroup,buy
        /// </summary>
        public string Mode { get; set; }

        public IEnumerable<SelectedProperty> SelectedProperties { get; set; }
    }

    public class SelectedProperty
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
