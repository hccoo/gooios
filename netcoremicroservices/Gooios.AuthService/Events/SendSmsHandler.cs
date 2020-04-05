using Gooios.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Events
{
    //[HandlesAsynchronously]
    //public class SendSmsHandler : IEventHandler<VerificationCreatedEvent>
    //{
    //    readonly ISmsProxy _smsProxy;

    //    public SendSmsHandler()
    //    {
    //        _smsProxy = IocProvider.GetService<ISmsProxy>();
    //    }

    //    public void Handle(VerificationCreatedEvent evnt)
    //    {
    //        string templateId = "";
    //        var verification = evnt.Source as Verification;
    //        if (verification.BizCode == BizCode.Register) templateId = "33592";

    //        if (verification.BizCode == BizCode.ForgetPassword) templateId = "33593";

    //        if(!string.IsNullOrEmpty(templateId))
    //            _smsProxy.SendVerificationCode(verification.Code, verification.To, templateId);
    //    }
    //}
}
