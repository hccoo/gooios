using Gooios.AuthorizationService.Models;
using Gooios.AuthorizationService.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Core
{
    public class SessionKeyValidator : IResourceOwnerPasswordValidator
    {
        private IAppletUserService loginUserService;

        public SessionKeyValidator(IAppletUserService _loginUserService)
        {
            this.loginUserService = _loginUserService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            AppUser loginUser = null;
            
            bool isAuthenticated = loginUserService.Authenticate(context.UserName, context.Password, out loginUser);
            if (!isAuthenticated)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid client credential");
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim("UserId", loginUser?.UserId??""),
                        new Claim("Name", context.UserName),
                        new Claim("NickName", loginUser?.NickName??"")
                    }
                );
            }

            return Task.CompletedTask;
        }
    }

    public class CookAppSessionKeyValidator : IResourceOwnerPasswordValidator
    {
        private IAppletUserService loginUserService;

        public CookAppSessionKeyValidator(IAppletUserService _loginUserService)
        {
            this.loginUserService = _loginUserService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            AppUser loginUser = null;
            var key = (context.Password == ""|| context.Password == "-") ? "cookwechat" : "cook";
            bool isAuthenticated = loginUserService.Authenticate(context.UserName, context.Password, out loginUser,key,context.UserName);
            if (!isAuthenticated)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid client credential");
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim("UserId", loginUser?.UserId??""),
                        new Claim("Name", context.UserName),
                        new Claim("NickName", loginUser?.NickName??"")
                    }
                );
            }

            return Task.CompletedTask;
        }
    }

    public class ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            context.IssuedClaims = claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
