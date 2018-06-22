using Gooios.ActivityService.Applications.DTOs;
using Gooios.ActivityService.Domains.Repositories;
using Gooios.ActivityService.Proxies;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Applications.Services
{
    public interface IGrouponActivityAppService : IApplicationServiceContract
    {
        GrouponActivityDTO CreateGrouponActivity(GrouponActivityDTO model, string userId);

        Task<IEnumerable<GrouponActivityDTO>> Get(string productId, string productMark);

        Task<GrouponActivityDTO> Get(string id);
    }

    public class GrouponActivityAppService : ApplicationServiceContract, IGrouponActivityAppService
    {
        readonly IGrouponActivityRepository _grouponActivityRepository;
        readonly IGrouponParticipationRepository _grouponParticipationRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IAuthServiceProxy _authServiceProxy;
        readonly IImageServiceProxy _imgServiceProxy;

        public GrouponActivityAppService(IGrouponActivityRepository grouponActivityRepository, IGrouponParticipationRepository grouponParticipationRepository, IDbUnitOfWork dbUnitOfWork, IAuthServiceProxy authServiceProxy, IImageServiceProxy imgServiceProxy)
        {
            _grouponActivityRepository = grouponActivityRepository;
            _grouponParticipationRepository = grouponParticipationRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _authServiceProxy = authServiceProxy;
            _imgServiceProxy = imgServiceProxy;
        }

        public GrouponActivityDTO CreateGrouponActivity(GrouponActivityDTO model, string userId)
        {
            var obj = new Domains.Aggregates.GrouponActivity
            {
                Count = model.Count,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                End = model.End,
                ProductId = model.ProductId,
                ProductMark = model.ProductMark,
                LastUpdOn = DateTime.Now,
                Start = model.Start,
                UnitPrice = model.UnitPrice
            };

            obj.GenerateId();
            obj.InitStatus();

            _grouponActivityRepository.Add(obj);
            _dbUnitOfWork.Commit();

            model.Id = obj.Id;

            return model;
        }

        public async Task<IEnumerable<GrouponActivityDTO>> Get(string productId, string productMark)
        {
            var ret = new List<GrouponActivityDTO>();
            var now = DateTime.Now;
            var result = _grouponActivityRepository.GetFiltered(o => (o.ProductId == productId && o.ProductMark == productMark) && o.Status == Domains.Aggregates.ActivityStatus.InProgress && o.Start <= now && o.End >= now);
            foreach (var item in result)
            {
                var participations = _grouponParticipationRepository.GetFiltered(g => g.GrouponActivityId == item.Id).Select(a =>
                {
                    return new GrouponParticipationDTO
                    {
                        BuyCount = a.BuyCount,
                        GrouponActivityId = a.GrouponActivityId,
                        OrderId = a.OrderId,
                        ParticipateTime = a.ParticipateTime.ToString("yyyy-MM-dd HH:mm"),
                        UserId = a.UserId
                    };
                }).ToList();

                var creator = await _authServiceProxy.GetUser(item.CreatedBy);

                ret.Add(new GrouponActivityDTO
                {
                    Id = item.Id,
                    Count = item.Count,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    End = item.End,
                    ProductId = item.ProductId,
                    Start = item.Start,
                    Status = item.Status,
                    UnitPrice = item.UnitPrice,
                    GrouponParticipations = participations,
                    CreatorName = creator?.NickName,
                    CreatorPortraitUrl = creator?.PortraitUrl
                });
            };
            return ret;
        }

        public async Task<GrouponActivityDTO> Get(string id)
        {
            var result = _grouponActivityRepository.Get(id);

            if (result == null) return null;

            var participations = _grouponParticipationRepository.GetFiltered(g => g.GrouponActivityId == id);
            var ps = new List<GrouponParticipationDTO>();
            foreach (var a in participations)
            {
                var user = await _authServiceProxy.GetUser(a.UserId);

                ps.Add(new GrouponParticipationDTO
                {
                    BuyCount = a.BuyCount,
                    GrouponActivityId = a.GrouponActivityId,
                    OrderId = a.OrderId,
                    ParticipateTime = a.ParticipateTime.ToString("yyyy-MM-dd HH:mm"),
                    UserId = a.UserId,
                    NickName = user?.NickName,
                    UserPortraitUrl = user?.PortraitUrl
                });
            };

            var creator = await _authServiceProxy.GetUser(result.CreatedBy);

            return new GrouponActivityDTO
            {
                Id = result.Id,
                Count = result.Count,
                CreatedBy = result.CreatedBy,
                CreatedOn = result.CreatedOn,
                End = result.End,
                ProductId = result.ProductId,
                Start = result.Start,
                Status = result.Status,
                UnitPrice = result.UnitPrice,
                GrouponParticipations = ps,
                CreatorName = creator?.NickName,
                CreatorPortraitUrl = creator?.PortraitUrl
            };
        }
    }
}
