using Gooios.AuthorizationService.Data;
using Gooios.AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;
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
    }
}
