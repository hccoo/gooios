using System;
using System.Text;

namespace Gooios.UserService.Domain
{
    public static class VerificationCodeGenerator
    {
        public static string GenerateVerificationCode(short bit)
        {
            var arr = new char[] { '0','1','2','3','4','5','6','7','8','9' };

            var result = new StringBuilder();

            var random = new Random();
            for (var i =0;i<bit;i++)
            {
                var index = random.Next(0, 10);
                result.Append(arr[index]);
            }

            return result.ToString();
        }
    }
}
