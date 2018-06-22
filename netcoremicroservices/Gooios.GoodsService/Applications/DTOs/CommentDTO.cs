using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.DTOs
{
    public class CommentDTO
    {
        public string GoodsId { get; set; }

        public string OrderId { get; set; }

        public string Content { get; set; }
        
        public IEnumerable<string> ImageIds { get; set; }
        
        public IEnumerable<string> TagIds { get; set; }

        public string NickName { get; set; }

        public string PortraitUrl { get; set; }

        public string CreateTime { get; set; }
    }
}
