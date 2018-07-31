using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Gooios.AuthorizationService.Services;
using Microsoft.Extensions.Logging;
using Gooios.AuthorizationService.Models;
using System.Net.Http;
using Gooios.AuthorizationService.Models.UserModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using Gooios.AuthorizationService.Proxies;
using Microsoft.AspNetCore.Authorization;
using Gooios.AuthorizationService.Data;
using Gooios.AuthorizationService.Attributes;
using Gooios.AuthorizationService.Configurations;

namespace Gooios.AuthorizationService.ApiControllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IVerificationProxy _verificationProxy;
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceConfigurationProxy _config;
        private readonly IAppletUserService _appletUserService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILogger<UserController> logger,
            IVerificationProxy verificationProxy,
            ApplicationDbContext dbContext,
            IServiceConfigurationProxy config,
            IAppletUserService appletUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _verificationProxy = verificationProxy;
            _dbContext = dbContext;
            _config = config;
            _appletUserService = appletUserService;
        }
        [TempData]
        public string ErrorMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterRequestModel model)
        {
            model.RoleName = "GooiosVip0";
            if (ModelState.IsValid)
            {
                //check the verification code
                var verification = await _verificationProxy.GetVerification(BizCode.Register, model.Mobile);
                if (verification == null)
                    return new BadRequestObjectResult("验证码不正确.");

                if (verification.Code != model.VerificationCode) return new BadRequestObjectResult("验证码不正确.");

                var role = await _roleManager.FindByNameAsync(model.RoleName);

                if (role == null) return new BadRequestObjectResult("找不到指定角色.");

                var user = new ApplicationUser { UserName = model.Mobile, PhoneNumber = model.Mobile };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _verificationProxy.SetVerificationUsed(verification);
                    var r = await _userManager.AddToRoleAsync(user, model.RoleName);
                    return new OkResult();
                }
                else
                    return new BadRequestObjectResult("创建用户失败.");
            }
            else
            {
                var errors = ModelState.Values.Select(o => o.Errors);
                var messageBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    string errMsg = string.Join(",", error.Select(o => o.ErrorMessage));
                    messageBuilder.Append(errMsg);
                    messageBuilder.Append(";");
                }

                return new BadRequestObjectResult($"参数错误: {messageBuilder.ToString()}");
            }
        }

        [ApiKey]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ModifyUserRequestModel model)
        {
            model.UserId = Request.Headers["userId"].FirstOrDefault();

            var u = _dbContext.ApplicationUsers.FirstOrDefault(o => o.Id == model.UserId);
            var au = _appletUserService.GetAppletUser(model.UserId);

            if (u != null)
            {
                u.NickName = model.NickName;
                u.PortraitUrl = model.PortraitUrl;
                await _userManager.UpdateAsync(u);
                _dbContext.SaveChanges();
                return new OkResult();
            }
            if (au != null)
            {
                au.NickName = model.NickName;
                au.UserPortrait = model.PortraitUrl;
                _appletUserService.AddOrUpdateAppletUser(au);
                _dbContext.SaveChanges();
                return new OkResult();
            }
            return new NotFoundResult();
        }

        [ApiKey]
        [HttpGet]
        public async Task<ApplicationUser> Get(string userId)
        {
            return _dbContext.ApplicationUsers.FirstOrDefault(o => o.Id == userId);
        }

        [ApiKey]
        [HttpGet]
        [Route("me")]
        public async Task<ApplicationUser> GetMe()
        {
            var userId = Request.Headers["userId"].FirstOrDefault();
            return _dbContext.ApplicationUsers.FirstOrDefault(o => o.Id == userId);
        }

        [ApiKey]
        [HttpGet]
        [Route("appletuser")]
        public async Task<AppletUser> GetAppletUserByOpenId(string openId)
        {
            return _appletUserService.GetAppletUser(openId);
        }

        [ApiKey]
        [HttpPost]
        [Route("appletusersession")]
        public AppletUserSession AddAppletUserSession([FromBody]AppletUserSession model)
        {
            return _appletUserService.AddOrUpdateAppletUserSession(model);
        }

        [ApiKey]
        [HttpPost]
        [Route("appletuser")]
        public void AddAppletUser([FromBody]AppletUser model)
        {
            _appletUserService.AddOrUpdateAppletUser(model);
        }

    }
}