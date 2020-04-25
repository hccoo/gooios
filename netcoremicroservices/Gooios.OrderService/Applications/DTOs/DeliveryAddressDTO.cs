using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Applications.DTOs
{
    public class DeliveryAddressDTO
    {
        public string Id { get; set; }

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
    }
}
