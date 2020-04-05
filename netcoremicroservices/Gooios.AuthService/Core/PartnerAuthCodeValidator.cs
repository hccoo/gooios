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
    public class PartnerAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly IUserServiceProxy _userServiceProxy;
        public PartnerAuthCodeValidator(IUserServiceProxy userServiceProxy)
        {
            _userServiceProxy = userServiceProxy;
        }

        public string GrantType => "partner_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var partner = context.Request.Raw["auth_partner"];//wechat alipay
            var authCode = context.Request.Raw["partner_auth_code"]; //合作方授权码

            var errorvalidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrWhiteSpace(partner) || string.IsNullOrWhiteSpace(authCode))
            {
                context.Result = errorvalidationResult;
                return;
            }

            var obj = await _userServiceProxy.VerifyCookAppPartnerLoginUserByAuthCode(authCode);

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
