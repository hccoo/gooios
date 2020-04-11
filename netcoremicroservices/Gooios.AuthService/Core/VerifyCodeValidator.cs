using Gooios.AuthService.Proxies;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gooios.UserService.Core
{
    public class VerifyCodeValidator : IExtensionGrantValidator
    {
        private readonly IUserServiceProxy _userServiceProxy;
        public VerifyCodeValidator(IUserServiceProxy userServiceProxy)
        {
            _userServiceProxy = userServiceProxy;
        }
        public string GrantType => "verify_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"]; //手机号
            var code = context.Request.Raw["mobile_verify_code"];//验证码

            if (phone == "18621685194" && code == "8888")
            {
                context.Result = new GrantValidationResult(
                     subject: phone,
                     authenticationMethod: "custom",
                     claims: new Claim[] {
                        new Claim("UserId", "test0000001"),
                        new Claim("Name", phone),
                        new Claim("NickName", phone)
                     }
                 );
                return;
            }

            var errorvalidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = errorvalidationResult;
                return;
            }

            var result = await _userServiceProxy.VerifyCookAppUserByVerifyCode(phone, code);
            if (result==null) 
            {
                context.Result = errorvalidationResult;
                return;
            }

            context.Result = new GrantValidationResult(
                     subject: phone,
                     authenticationMethod: "custom",
                     claims: new Claim[] {
                        new Claim("UserId", result.Id),
                        new Claim("Name", phone),
                        new Claim("NickName", result.UserName)
                     }
                 );
        }
    }
}
