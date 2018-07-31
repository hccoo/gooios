using Gooios.AuthorizationService.Models;

namespace Gooios.AuthorizationService.Services
{
    public interface IAppletUserService
    {
        bool Authenticate(string openId, string sessionKey, out AppUser appUser);

        void AddOrUpdateAppletUser(AppletUser model);

        AppletUserSession AddOrUpdateAppletUserSession(AppletUserSession model);

        AppletUser GetAppletUser(string openId);

        AppletUserSession GetAppletUserSessionBySessionKey(string sessionKey);

        AppletUserSession GetAppletUserSessionByOpendId(string openId);

    }
}
