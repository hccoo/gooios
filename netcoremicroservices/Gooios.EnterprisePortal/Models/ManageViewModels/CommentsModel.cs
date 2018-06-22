using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class CommentsModel
    {
        public string ReservationId { get; set; }

        public string OrderId { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> ImageIds { get; set; }

        public IEnumerable<CommentImage> Images { get; set; }

        public IEnumerable<string> TagIds { get; set; }
    }

    public class CommentImage
    {
        public int Id { get; set; }

        public string CommentId { get; set; }

        public string ImageId { get; set; }

        public string HttpPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
