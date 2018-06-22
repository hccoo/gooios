using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Domains.Aggregates
{
    public class Address : ValueObject<Address>
    {
        public Address() { }

        public Address(string province, string city, string area, string streetAddress, string postcode, double latitude, double longitude)
        {
            Province = province;
            City = city;
            Area = area;
            StreetAddress = streetAddress;
            Postcode = postcode;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
