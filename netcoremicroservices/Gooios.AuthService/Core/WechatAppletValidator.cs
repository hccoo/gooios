using Gooios.AuthService.Proxies;
using Gooios.AuthService.Proxies.Dtos;
using Gooios.UserService.Proxies;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gooios.AuthService.Core
{
    public class WechatAppletValidator : IExtensionGrantValidator
    {
        private readonly IUserServiceProxy _userServiceProxy;
        public WechatAppletValidator(IUserServiceProxy userServiceProxy)
        {
            _userServiceProxy = userServiceProxy;
        }
        public string GrantType => "wechat_applet";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var code = context.Request.Raw["wechat_code"];

            var errorvalidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            var obj = await _userServiceProxy.VerifyWechatAppletLoginUserByCode(code);

            if (obj == null)
            {
                context.Result = errorvalidationResult;
                return;
            }

            context.Result = new GrantValidationResult(
                     subject: obj.PartnerKey,
                     authenticationMethod: "custom",
                     claims: new Claim[] {
                        new Claim("UserId", obj.Id),
                        new Claim("Name",obj.PartnerKey),
                        new Claim("NickName", obj.PartnerKey)
                     }
                 );
        }
    }

}
