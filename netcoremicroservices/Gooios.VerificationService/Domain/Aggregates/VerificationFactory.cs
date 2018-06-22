using System;

namespace Gooios.VerificationService.Domain.Aggregates
{
    public static class VerificationFactory
    {
        public static Verification CreateVerification(BizCode bizCode, string to)
        {
            var result = new Verification();
            result.GenerateNewCode();

            var now = DateTime.Now;

            result.BizCode = bizCode;
            result.CreatedOn = now;
            result.ExpiredOn = now.AddHours(1);
            result.IsSuspend = result.IsUsed = false;
            result.To = to;

            return result;
        }
    }
}
