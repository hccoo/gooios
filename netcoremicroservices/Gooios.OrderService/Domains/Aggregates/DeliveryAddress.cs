using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public class DeliveryAddress : Entity<string>
    {
        public string UserId { get; set; }

        public string LinkMan { get; set; }

        /// <summary>
        /// 性别：1-男 2-女
        /// </summary>
        public int Gender { get; set; }

        public string Mobile { get; set; }

        public string Mark { get; set; }

        public bool IsDefault { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class DeliveryAddressFactory
    {
        public static DeliveryAddress CreateInstance(string userId, string linkman, int gender, string mobile, string mark, bool isDefault, string province, string city, string area, string streeAddress,
            string postcode, string createdBy)
        {
            var obj = new DeliveryAddress
            {
                Area = area,
                UserId = userId,
                StreetAddress = streeAddress,
                Province = province,
                Postcode = postcode,
                Mobile = mobile,
                City = city,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                Gender = gender,
                IsDefault = isDefault,
                LinkMan = linkman,
                Mark = mark
            };
            obj.GenerateId();
            return obj;
        }
    }
}
