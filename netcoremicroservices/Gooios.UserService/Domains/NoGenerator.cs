using System;

namespace Gooios.UserService.Domain
{
    public static class NoGenerator
    {
        /// <summary>
        /// 年月日订单流水
        /// 180312-1803110001
        /// 自营：100000
        /// </summary>
        /// <returns></returns>
        public static string GenerateNo()
        {
            return $"{DateTime.Now.ToString("yyMMdd")}{Guid.NewGuid().GetHashCode().ToString()}";
        }
    }
}
