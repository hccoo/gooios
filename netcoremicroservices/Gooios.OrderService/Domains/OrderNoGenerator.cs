﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains
{
    public static class OrderNoGenerator
    {
        /// <summary>
        /// 年月日订单流水
        /// 180312-1803110001
        /// 自营：100000
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNo()
        {
            return $"{DateTime.Now.ToString("yyMMdd")}{Guid.NewGuid().GetHashCode().ToString()}";
        }
    }
}
