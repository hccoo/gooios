using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Domains.Aggregates
{
    public static class CommentFactory
    {
        public static Comment CreateComment(
            string reservationId,
            string orderId,
            string content,
            string userId,
            IEnumerable<string> imageIds,
            IEnumerable<CommentTag> commentTags)
        {
            var result = new Comment
            {
                ReservationId = reservationId,
                OrderId = orderId,
                Content = content,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ImageIds = imageIds,
                CommentTags = commentTags
            };

            result.GenerateId();

            result.CommentTags.ToList().ForEach(item=> { item.CommentId = result.Id; });

            return result;
        }
    }
}
