using Gooios.AuthorizationService.Data;
using Gooios.AuthorizationService.Models;
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

        public AppletUserService(ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
        }

        public void AddOrUpdateAppletUser(AppletUser model)
        {
            var obj = AppletUserFactory.CreateInstance(model.OpenId, model.OrganizationId, model.ApplicationId, model.Channel, model.NickName, model.UserPortrait, model.UserId);

            var usr = _dbContext.AppletUsers.FirstOrDefault(o=>o.OpenId==model.OpenId);
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

        public bool Authenticate(string userName, string password, out AppUser appUser)
        {
            appUser = null;

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

        public AppletUser GetAppletUser(string openId)
        {
            return _dbContext.AppletUsers.FirstOrDefault(o => o.OpenId == openId);
        }

        public AppletUserSession GetAppletUserSessionByOpendId(string openId)
        {
            return _dbContext.AppletUserSessions.FirstOrDefault(o=>o.OpenId==openId);
        }

        public AppletUserSession GetAppletUserSessionBySessionKey(string sessionKey)
        {
            return _dbContext.AppletUserSessions.FirstOrDefault(o=>o.SessionKey==sessionKey);
        }
    }
}
