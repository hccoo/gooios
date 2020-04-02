using Gooios.AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Services
{
    public interface IPartnerLoginService
    {
        bool AddPartnerLogin(PartnerLogin model);
    }

    public class PartnerLoginService : IPartnerLoginService
    {
        public bool AddPartnerLogin(PartnerLogin model)
        {
            throw new NotImplementedException();
        }
    }
}
