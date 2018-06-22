using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Domains.Aggregates;
using Gooios.FancyService.Domains.Repositories;
using Gooios.FancyService.Proxies;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.FancyService.Applications.Services
{
    public interface ICommentAppService : IApplicationServiceContract
    {
        void AddComment(CommentDTO comment, string operatorId);

        Task<IEnumerable<CommentDTO>> GetByServiceId(string serviceId, int pageIndex, int pageSize);

        Task<IEnumerable<CommentDTO>> GetByServicerId(string servicerId, int pageIndex, int pageSize);
    }
    public class CommentAppService : ApplicationServiceContract, ICommentAppService
    {
        readonly ICommentRepository _commentRepository;
        readonly ICommentTagRepository _commentTagRepository;
        readonly ICommentImageRepository _commentImageRepository;
        readonly IReservationRepository _reservationRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IImageServiceProxy _imageServiceProxy;
        readonly IAuthServiceProxy _authServiceProxy;

        public CommentAppService(
            IDbUnitOfWork dbUnitOfWork,
            ICommentRepository commentRepository,
            ICommentTagRepository commentTagRepository,
            ICommentImageRepository commentImageRepository,
            IReservationRepository reservationRepository,
            IImageServiceProxy imageServiceProxy,
            IAuthServiceProxy authServiceProxy)
        {
            _commentRepository = commentRepository;
            _commentTagRepository = commentTagRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _commentImageRepository = commentImageRepository;
            _reservationRepository = reservationRepository;
            _imageServiceProxy = imageServiceProxy;
            _authServiceProxy = authServiceProxy;
        }

        public void AddComment(CommentDTO comment, string operatorId)
        {
            var commentTags = comment.TagIds?.Select(item => new CommentTag { ReservationId = comment.ReservationId, TagId = item, UserId = operatorId })?.ToList() ?? new List<CommentTag>();

            var commentCount = _commentRepository.GetFiltered(o => o.OrderId == comment.OrderId).Count();

            if (commentCount > 0) throw new Exception("已经评论过，一个订单只能评论一次.");

            //TODO: Get order from order service, if not exist, throw an exception.

            var obj = CommentFactory.CreateComment(comment.ReservationId, comment.OrderId, comment.Content, operatorId, comment.ImageIds, commentTags);

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

        public async Task<IEnumerable<CommentDTO>> GetByServiceId(string serviceId, int pageIndex, int pageSize)
        {
            var objs = _commentRepository.GetCommentsByServiceId(serviceId, pageIndex, pageSize);
            var result = new List<CommentDTO>();
            foreach (var item in objs)
            {
                var imgIds = _commentImageRepository.GetFiltered(o => o.CommentId == item.Id).Select(g => g.ImageId).ToList();

                var userTask = _authServiceProxy.GetUser(item.CreatedBy);
                var imgs = await _imageServiceProxy.GetImagesByIds(imgIds);
                var user = await userTask;

                result.Add(new CommentDTO
                {
                    Content = item.Content,
                    OrderId = item.OrderId,
                    CommentOwnerUserNickName = user?.NickName,
                    PortraitImageUrl = user?.PortraitUrl,
                    ReservationId = item.ReservationId,
                    ImageIds = imgIds,
                    Images = imgs?.Select(obj => new CommentImageDTO
                    {
                        Title = obj.Title,
                        ImageId = obj.Id,
                        CommentId = item.Id,
                        Description = obj.Description,
                        HttpPath = obj.HttpPath
                    }),
                    CreatedOn=item.CreatedOn.ToString("yyyy-MM-dd")
                });
            }
            return result;
        }

        public async Task<IEnumerable<CommentDTO>> GetByServicerId(string servicerId, int pageIndex, int pageSize)
        {
            var objs = _commentRepository.GetCommentsByServicerId(servicerId, pageIndex, pageSize);
            var result = new List<CommentDTO>();
            foreach (var item in objs)
            {
                var imgIds = _commentImageRepository.GetFiltered(o => o.CommentId == item.Id).Select(g => g.ImageId).ToList();

                var userTask = _authServiceProxy.GetUser(item.CreatedBy);
                var imgs = await _imageServiceProxy.GetImagesByIds(imgIds);
                var user = await userTask;

                result.Add(new CommentDTO
                {
                    Content = item.Content,
                    OrderId = item.OrderId,
                    CommentOwnerUserNickName = user?.NickName,
                    PortraitImageUrl = user?.PortraitUrl,
                    ReservationId = item.ReservationId,
                    ImageIds = imgIds,
                    Images = imgs?.Select(obj => new CommentImageDTO
                    {
                        Title = obj.Title,
                        ImageId = obj.Id,
                        CommentId = item.Id,
                        Description = obj.Description,
                        HttpPath = obj.HttpPath
                    }),
                    CreatedOn = item.CreatedOn.ToString("yyyy-MM-dd")
                });
            }
            return result;
        }
    }
}
