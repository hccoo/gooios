﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models.UserModels
{
    public class ResetPasswordModel
    {
        public string Mobile { get; set; }

        public string VerificationCode { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
