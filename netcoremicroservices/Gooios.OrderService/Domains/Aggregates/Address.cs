using Gooios.OrderService.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrderService.Domains.Aggregates
{
    public class Address : ValueObject<Address>
    {
        public Address() { }

        public Address(string province, string city, string area, string streetAddress, string postcode)
        {
            Province = province;
            City = city;
            Area = area;
            StreetAddress = streetAddress;
            Postcode = postcode;
        }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }
    }
}
