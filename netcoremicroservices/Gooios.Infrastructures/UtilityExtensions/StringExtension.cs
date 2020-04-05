using System;
using System.Security.Cryptography;
using System.Text;

namespace Gooios.Infrastructure.UtilityExtensions
{
    public static class StringExtension
    {
        public static string ToMD5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
