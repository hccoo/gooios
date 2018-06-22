using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.OrganizationService.Domains.Aggregates
{
    public class Organization : Entity<string>
    {
        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string CertificateNo { get; set; }

        public string Introduction { get; set; }

        public string Phone { get; set; }

        public string CustomServicePhone { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Postcode { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastUpdBy { get; set; }

        public DateTime? LastUpdOn { get; set; }

        public bool IsSuspend { get; set; }

        public string LogoImageId { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void SetSuspend()
        {
            IsSuspend = true;
        }

        public void InitAddress()
        {
            Area = Address.Area;
            City = Address.City;
            Postcode = Address.Postcode;
            Province = Address.Province;
            StreetAddress = Address.StreetAddress;
            Latitude = Address.Latitude;
            Longitude = Address.Longitude;
        }

        public void ResolveAddress()
        {
            if (Address == null) Address = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }
    }
}
