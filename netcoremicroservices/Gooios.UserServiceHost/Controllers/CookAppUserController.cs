using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gooios.UserService.Application.Services;
using Gooios.UserService.Applications.Dtos;
using Gooios.UserServiceHost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gooios.UserServiceHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookAppUserController : BaseController
    {
        private readonly IUserAppService _userAppService;

        public CookAppUserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost]
        [Route("verifybypassword/v1")]
        public async Task<CookAppUserDto> VerifyCookAppUserByPassword(VerifyCookAppUserByPasswordModel model) 
            => _userAppService.VerifyCookAppUserByPassword(model.UserName, model.Password);

        [HttpPost]
        [Route("verifybycode/v1")]
        public async Task<CookAppUserDto> VerifyCookAppUserByVerifyCode(VerifyCookAppUserByVerifyCodeModel model)
            => await _userAppService.VerifyCookAppUserByVerifyCode(model.UserName, model.Code);

        [HttpPost]
        [Route("verifybyauthcode/v1")]
        public async Task<CookAppPartnerLoginUserDto> VerifyCookAppPartnerLoginUserByAuthCode(VerifyCookAppPartnerLoginUserByAuthCodeModel model)
            => await _userAppService.VerifyCookAppPartnerLoginUserByAuthCode(model.AuthorizationCode);

        [HttpPost]
        [Route("verifybywechatapplet/v1")]
        public async Task<CookAppPartnerLoginUserDto> VerifyWechatAppletLoginUserByCode(VerifyCookAppPartnerLoginUserByAuthCodeModel model)
            => await _userAppService.VerifyWechatAppletLoginUserByCode(model.AuthorizationCode);

        [HttpPost]
        [Route("v1")]
        public async Task<string> AddCookAppUser(AddCookAppUserModel model)
        {
            var ret = _userAppService.AddCookAppUser(model.UserName, model.Password, model.Mobile, model.Email);
            return ret?.Id ?? "";
        }

        [HttpPost]
        [Route("setservicerid/v1")]
        public bool SetServicerIdForUser(SetServicerIdForUserModel model)
        {
            return _userAppService.SetServicerIdForUser(model.UserName, model.ServicerId);
        }

        [HttpGet]
        [Route("v1")]
        public CookAppUserDto GetUser(string idOrUserName)
        {
            return _userAppService.GetUser(idOrUserName);
        }

        [HttpGet]
        [Route("partnerloginuser/v1")]
        public CookAppPartnerLoginUserDto GetPartnerLoginUser()
        {
            return _userAppService.GetPartnerLoginUser(UserId);
        }

        [HttpGet]
        [Route("currentuser/v1")]
        public CookAppUserDto GetCurrentUser()
        {
            return _userAppService.GetUser(UserId);
        }
    }
}