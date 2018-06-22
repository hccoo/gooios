using Gooios.GoodsService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    /// <summary>
    /// 暂时不用，后期需要加入
    /// </summary>
    public class GoodsNumber : ValueObject<GoodsNumber>
    {
        public GoodsNumber(string number)
        {
            Number = number;
        }

        public string Number { get; set; }
    }
}
