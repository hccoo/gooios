using Gooios.VerificationService.Domain.Events;
using System;

namespace Gooios.VerificationService.Domain.Aggregates
{
    public class Verification : Entity<int>
    {
        public string Code { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public DateTime ExpiredOn { get; set; } = DateTime.Now.AddHours(1);

        public BizCode BizCode { get; set; } = default(int);

        public bool IsUsed { get; set; } = false;

        public bool IsSuspend { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime LastUpdOn { get; set; } = DateTime.Now;

        //public VerificationStatus Status { get; set; } = VerificationStatus.Init;

        public void SetSuspend()
        {
            IsSuspend = true;
            LastUpdOn = DateTime.Now;
        }

        public void SetUsed()
        {
            IsSuspend = true;
            IsUsed = true;
            LastUpdOn = DateTime.Now;
        }

        public void GenerateNewCode()
        {
            Code = VerificationCodeGenerator.GenerateVerificationCode(4);
        }

        public void CreatedConfirm()
        {
            DomainEvent.Publish<VerificationCreatedEvent>(new VerificationCreatedEvent(this) { CreatedTime = DateTime.Now, ID = Guid.NewGuid(), TimeStamp = DateTime.Now, VerificationCode = this.Code, VerificationTo = this.To, BizCode = this.BizCode });
        }
    }

    //public enum VerificationStatus
    //{
    //    Init = 0,
    //    Sent = 2
    //}

    public enum BizCode
    {
        Register = 1,
        ForgetPassword = 2
    }
}
