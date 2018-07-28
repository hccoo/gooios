using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Services
{
    public interface IWeChatService
    {
        Task<string> GetOpenId(string code, string organizationId = null);
    }

    public class WeChatService : IWeChatService
    {
        public Task<string> GetOpenId(string code, string organizationId = null)
        {
            throw new NotImplementedException();
        }
    }
}
