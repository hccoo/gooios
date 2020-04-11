using Gooios.Infrastructure.Secrets;
using Gooios.UserService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Domain.Aggregates
{
    public class CookAppUser : Entity<string>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string ServicerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class CookAppUserFactory
    {
        public static CookAppUser CreateInstance(string userName, string password, string mobile, string email)
        {
            var now = DateTime.Now;
            var user = new CookAppUser
            {
                UserName = userName,
                Password = SecretProvider.EncryptToMD5(password),
                Mobile = mobile,
                Email = email,
                CreatedBy = userName,
                CreatedOn = now,
                UpdatedBy = userName,
                UpdatedOn = now
            };
            user.GenerateId();
            return user;
        }
    }
}
