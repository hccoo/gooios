using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models.UserModels
{
    public class ModifyUserRequestModel
    {
        public string UserId { get; set; }

        public string NickName { get; set; }

        public string PortraitUrl { get; set; }
    }
}
