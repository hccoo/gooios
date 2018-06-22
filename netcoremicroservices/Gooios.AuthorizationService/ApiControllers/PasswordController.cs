using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Gooios.AuthorizationService.Models;
using Gooios.AuthorizationService.Models.UserModels;
using Gooios.AuthorizationService.Proxies;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using Gooios.AuthorizationService.Attributes;
using Gooios.AuthorizationService.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gooios.AuthorizationService.ApiControllers
{
    [Route("api/password")]
    public class PasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationProxy _verificationProxy;
        private readonly ApplicationDbContext _dbContext;

        public PasswordController(UserManager<ApplicationUser> userManager, IVerificationProxy verificationProxy, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _verificationProxy = verificationProxy;
            _dbContext = dbContext;
        }

        [ApiKey]
        [HttpPut]
        [Route("change")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Patch([FromBody]ChangePasswordModel model)
        {
            if (model.ConfirmPassword != model.NewPassword) return new BadRequestObjectResult("密码与确认密码不匹配.");

            Request.Headers.TryGetValue("userid", out StringValues vals);
            var userId = vals.ToArray().GetValue(0)?.ToString();
            
            var user = _dbContext.ApplicationUsers.FirstOrDefault(o => o.Id == userId);

            if (user == null) return new BadRequestObjectResult("非法操作.");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if (!isPasswordCorrect) return new BadRequestObjectResult("原密码或账号错误.");

            await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return new OkResult();
        }

        [HttpPut]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            if (model.ConfirmPassword != model.NewPassword) return new BadRequestObjectResult("密码与确认密码不匹配.");

            var verification = await _verificationProxy.GetVerification(BizCode.ForgetPassword, model.Mobile);
            if (verification == null)
                return new BadRequestObjectResult("所提供的验证码不正确.");

            if (verification.Code != model.VerificationCode) return new BadRequestObjectResult("验证码不正确.");

            var user = await _userManager.FindByNameAsync(model.Mobile);

            if(user==null) new BadRequestObjectResult("指定的用户不存在.");

            var token= await _userManager.GeneratePasswordResetTokenAsync(user);

            var result  = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                await _verificationProxy.SetVerificationUsed(verification);
                return new OkResult();
            }
            else
                return new BadRequestObjectResult("重置密码失败.");

            return new OkResult();
        }
    }
}
