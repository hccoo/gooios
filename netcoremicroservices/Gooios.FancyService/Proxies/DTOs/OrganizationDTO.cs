using Gooios.FancyService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Proxies.DTOs
{
    public class OrganizationDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string CertificateNo { get; set; }

        public string Introduction { get; set; }

        public Address Address { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastUpdBy { get; set; }

        public DateTime? LastUpdOn { get; set; }

        public bool IsSuspend { get; set; }

        public string LogoImageId { get; set; }

        public string Phone { get; set; }

        public string CustomServicePhone { get; set; }
    }
}
