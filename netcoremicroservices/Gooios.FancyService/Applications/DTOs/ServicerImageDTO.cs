using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.DTOs
{
    public class ServicerImageDTO
    {
        public int Id { get; set; }

        public string ServicerId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
