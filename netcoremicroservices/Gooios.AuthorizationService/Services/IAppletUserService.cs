using Gooios.AuthorizationService.Models;

namespace Gooios.AuthorizationService.Services
{
    public interface IAppletUserService
    {
        bool Authenticate(string openId, string sessionKey, out AppUser appUser);
    }
}
