using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class ImageModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string HttpPath { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ImageBase64Content { get; set; } = string.Empty;

        public string ClientFileName { get; set; } = string.Empty;
    }
}
