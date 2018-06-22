using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.GoodsService.Proxies;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.Services
{
    public interface ICommentAppService : IApplicationServiceContract
    {
        void AddComment(CommentDTO comment, string operatorId);

        Task<IEnumerable<CommentDTO>> Get(string goodsId, int pageIndex, int pageSize);
    }
    public class CommentAppService : ApplicationServiceContract, ICommentAppService
    {
        readonly ICommentRepository _commentRepository;
        readonly ICommentTagRepository _commentTagRepository;
        readonly ICommentImageRepository _commentImageRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IAuthServiceProxy _authServiceProxy;

        public CommentAppService(IDbUnitOfWork dbUnitOfWork, ICommentRepository commentRepository, ICommentTagRepository commentTagRepository, ICommentImageRepository commentImageRepository, IAuthServiceProxy authServiceProxy)
        {
            _commentRepository = commentRepository;
            _commentTagRepository = commentTagRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _commentImageRepository = commentImageRepository;
            _authServiceProxy = authServiceProxy;
        }

        public void AddComment(CommentDTO comment, string operatorId)
        {
            var commentTags = comment.TagIds?.Select(item => new CommentTag { GoodsId = comment.GoodsId, TagId = item, UserId = operatorId })?.ToList() ?? new List<CommentTag>();

            var commentCount = _commentRepository.GetFiltered(o => o.OrderId == comment.OrderId).Count();

            if (commentCount > 0) throw new Exception("已经评论过，一个订单只能评论一次.");

            //TODO: Get order from order service, if not exist, throw an exception.

            var obj = CommentFactory.CreateComment(comment.GoodsId, comment.OrderId, comment.Content, operatorId, comment.ImageIds, commentTags);

            _commentRepository.Add(obj);

            obj.CommentTags.ToList().ForEach(item =>
            {
                _commentTagRepository.Add(item);
            });

            obj.ImageIds.ToList().ForEach(item =>
            {
                _commentImageRepository.Add(new CommentImage { CommentId = obj.Id, ImageId = item });
            });

            _dbUnitOfWork.Commit();
        }

        public async Task<IEnumerable<CommentDTO>> Get(string goodsId, int pageIndex, int pageSize)
        {
            var results = new List<CommentDTO>();
            var res = _commentRepository.GetFiltered(o => o.GoodsId == goodsId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            foreach (var item in res)
            {
                var user = await _authServiceProxy.GetUser(item.CreatedBy);
                results.Add(new CommentDTO
                {
                    Content = item.Content,
                    GoodsId = item.GoodsId,
                    OrderId = item.OrderId,
                    ImageIds = _commentImageRepository.GetFiltered(o => o.CommentId == item.Id).Select(o => o.ImageId).ToList(),
                    TagIds = _commentTagRepository.GetFiltered(o => o.CommentId == item.Id).Select(g => g.TagId).ToList(),
                    CreateTime = item.CreatedOn.ToString("yyyy-MM-dd"),
                    NickName = user?.NickName,
                    PortraitUrl = user?.PortraitUrl
                });
            }

            return results;
        }
    }
}
