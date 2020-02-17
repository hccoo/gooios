using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.AuthorizationService.Models.UserModels
{
    public class RegisterRequestModel
    {
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "手机号码必须为11位")]
        [Required]
        [Phone(ErrorMessage ="手机号码格式不正确")]
        [Display(Name = "手机号码")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }

        [StringLength(20, ErrorMessage = "密码长度必须为6~20位", MinimumLength = 6)]
        [Display(Name = "密码")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }
        
        public string RoleName { get; set; }
    }
    public class AddUserRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
