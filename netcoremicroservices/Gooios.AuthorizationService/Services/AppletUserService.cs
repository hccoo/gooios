using Gooios.AuthorizationService.Configurations;
using Gooios.AuthorizationService.Data;
using Gooios.AuthorizationService.Models;
using Gooios.AuthorizationService.Proxies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Services
{
    public class AppletUserService : IAppletUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationProxy _verificationProxy;
        private readonly IWechatProxy _wechatProxy;
        private readonly IServiceConfigurationProxy _config;

        public AppletUserService(ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            IVerificationProxy verificationProxy,
            UserManager<ApplicationUser> userManager,
            IWechatProxy wechatProxy,
            IServiceConfigurationProxy config)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _verificationProxy = verificationProxy;
            _userManager = userManager;
            _wechatProxy = wechatProxy;
            _config = config;
        }

        public void AddOrUpdateAppletUser(AppletUser model)
        {
            var obj = AppletUserFactory.CreateInstance(model.OpenId, model.OrganizationId, model.ApplicationId, model.Channel, model.NickName, model.UserPortrait, model.UserId);

            var usr = _dbContext.AppletUsers.FirstOrDefault(o => o.OpenId == model.OpenId);
            if (usr == null) _dbContext.AppletUsers.Add(obj);
            else
            {
                usr.LastUpdOn = DateTime.Now;
                usr.ApplicationId = model.ApplicationId;
                usr.Channel = model.Channel;
                usr.NickName = model.NickName;
                usr.OpenId = model.OpenId;
                usr.OrganizationId = model.OrganizationId;
                usr.UserId = model.UserId;
                usr.UserPortrait = model.UserPortrait;
                _dbContext.Update(usr);
            }
            _dbContext.SaveChanges();
        }

        public AppletUserSession AddOrUpdateAppletUserSession(AppletUserSession model)
        {
            var obj = UserSessionFactory.CreateInstance(model.UserId, model.OpenId, model.SessionKey);

            var session = _dbContext.AppletUserSessions.FirstOrDefault(o => o.OpenId == model.OpenId);
            if (session == null) _dbContext.AppletUserSessions.Add(obj);
            else
            {
                _dbContext.Remove(session);
                _dbContext.AppletUserSessions.Add(obj);
            }
            _dbContext.SaveChanges();
            return obj;
        }

        public bool Authenticate(string userName, string password, out AppUser appUser, string key = "", string authorizationCode = "")
        {
            appUser = null;

            if (key == "cook")
            {
                var user = _dbContext.ApplicationUsers.FirstOrDefault(o => o.UserName == userName);
                var result = _verificationProxy.GetVerification(BizCode.Login, userName).ConfigureAwait(false);
                var verification = result.GetAwaiter().GetResult();
                if (verification == null) return false;
                if (verification.Code != password) return false;

                if (user != null)
                {
                    appUser = new AppUser { NickName = user.NickName, UserId = user.Id, PortraitUrl = user.PortraitUrl };
                    return true;
                }
                else
                {
                    var usr = new ApplicationUser { UserName = userName, Email = "" };
                    var pwd = Guid.NewGuid().ToString().Substring(0, 8);
                    var cresult = _userManager.CreateAsync(usr, pwd).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (cresult.Succeeded)
                    {
                        var ret = _signInManager.PasswordSignInAsync(userName, pwd, true, false).ConfigureAwait(false);
                        var signRet = ret.GetAwaiter().GetResult();
                        return signRet?.Succeeded ?? false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (key == "cookwechat")
            {
                var obj = _wechatProxy.GetAccessToken(_config.WeChatAppId, _config.WeChatAppSecret, authorizationCode, "authorization_code", out ErrorResponseModel res).ConfigureAwait(false);
                var ret = obj.GetAwaiter().GetResult();
                var ent = _dbContext.PartnerLogins.Add(new PartnerLogin
                {
                    CreatedBy = ret.OpenId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    UpdatedBy = ret.OpenId,
                    AccessToken = ret.AccessToken,
                    AuthorizationCode = authorizationCode,
                    ExpiredIn = ret.ExpiresIn,
                    LoginChannel = LoginChannel.Wechat,
                    OpenId = ret.OpenId,
                    RefreshToken = ret.RefreshToken,
                    Scope = ret.Scope,
                    UnionId = ret.UnionId
                });

                return ent != null;
            }
            else
            {
                var appletUserSession = _dbContext.AppletUserSessions.FirstOrDefault(o => o.OpenId == userName && o.GooiosSessionKey == password);
                var appletUser = appletUserSession == null ? null : _dbContext.AppletUsers.FirstOrDefault(o => o.OpenId == appletUserSession.OpenId);

                if (appletUser != null)
                {
                    appUser = new AppUser { NickName = appletUser.NickName, PortraitUrl = appletUser.UserPortrait, UserId = appletUser.OpenId };
                }

                var result = _signInManager.PasswordSignInAsync(userName, password, true, false).ConfigureAwait(false);
                var signRet = result.GetAwaiter().GetResult();

                var user = _dbContext.ApplicationUsers.FirstOrDefault(o => o.UserName == userName);

                if (user != null)
                {
                    appUser = new AppUser { NickName = user.NickName, UserId = user.Id, PortraitUrl = user.PortraitUrl };
                }

                return appletUserSession == null ? (signRet?.Succeeded ?? false) : true;
            }
        }

        public AppletUser GetAppletUser(string openId)
        {
            return _dbContext.AppletUsers.FirstOrDefault(o => o.OpenId == openId);
        }

        public AppletUserSession GetAppletUserSessionByOpendId(string openId)
        {
            return _dbContext.AppletUserSessions.FirstOrDefault(o => o.OpenId == openId);
        }

        public AppletUserSession GetAppletUserSessionBySessionKey(string sessionKey)
        {
            return _dbContext.AppletUserSessions.FirstOrDefault(o => o.SessionKey == sessionKey);
        }
    }
}
