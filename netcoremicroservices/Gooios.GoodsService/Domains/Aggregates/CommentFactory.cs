using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Domains.Aggregates
{
    public static class CommentFactory
    {
        public static Comment CreateComment(
            string goodsId,
            string orderId,
            string content,
            string userId,
            IEnumerable<string> imageIds,
            IEnumerable<CommentTag> commentTags)
        {
            var result = new Comment
            {
                GoodsId = goodsId,
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
