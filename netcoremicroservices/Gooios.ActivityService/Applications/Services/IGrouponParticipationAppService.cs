using Gooios.ActivityService.Applications.DTOs;
using Gooios.ActivityService.Domains.Repositories;
using Gooios.ActivityService.Proxies;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Applications.Services
{
    public interface IGrouponParticipationAppService : IApplicationServiceContract
    {
        void CreateGrouponParticipation(GrouponParticipationDTO model);

        Task<IEnumerable<GrouponParticipationDTO>> Get(string activityId, int pageIndex, int pageSize);
    }

    public class GrouponParticipationAppService : ApplicationServiceContract, IGrouponParticipationAppService
    {
        readonly IGrouponActivityRepository _grouponActivityRepository;
        readonly IGrouponParticipationRepository _grouponParticipationRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly IAuthServiceProxy _authServiceProxy;

        public GrouponParticipationAppService(
            IGrouponActivityRepository grouponActivityRepository,
            IGrouponParticipationRepository grouponParticipationRepository,
            IDbUnitOfWork dbUnitOfWork,
            IEventBus eventBus,
            IAuthServiceProxy authServiceProxy
            )
        {
            _grouponActivityRepository = grouponActivityRepository;
            _grouponParticipationRepository = grouponParticipationRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _authServiceProxy = authServiceProxy;
        }

        public void CreateGrouponParticipation(GrouponParticipationDTO model)
        {
            var activity = _grouponActivityRepository.Get(model.GrouponActivityId);

            if (activity == null) throw new Exception("活动内容异常，无法找到指定的活动记录.");

            if (!activity.IsInProcess()) throw new Exception("活动不在进行中.");

            var objToAdd = new Domains.Aggregates.GrouponParticipation
            {
                BuyCount = model.BuyCount,
                GrouponActivityId = model.GrouponActivityId,
                OrderId = model.OrderId,
                ParticipateTime = DateTime.Now,
                UserId = model.UserId
            };

            _grouponParticipationRepository.Add(objToAdd);

            _dbUnitOfWork.Commit();

            objToAdd.ConfirmParticipated();
            _eventBus.Commit();
            //using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            //{
            //    coordinator.Commit();
            //}
        }

        public async Task<IEnumerable<GrouponParticipationDTO>> Get(string activityId, int pageIndex, int pageSize)
        {
            var result = new List<GrouponParticipationDTO>();
            var participations = _grouponParticipationRepository
                .GetFiltered(o => o.GrouponActivityId == activityId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            foreach (var item in participations)
            {
                var user = await _authServiceProxy.GetUser(item.UserId);
                var nickName = user?.NickName ?? string.Empty;
                var userPortraitUrl = user?.PortraitUrl ?? string.Empty;
                result.Add(new GrouponParticipationDTO
                {
                    BuyCount = item.BuyCount,
                    GrouponActivityId = item.GrouponActivityId,
                    NickName = nickName,
                    OrderId = item.OrderId,
                    ParticipateTime = item.ParticipateTime.ToString("yyyy-MM-dd HH:mm"),
                    UserId = item.UserId,
                    UserPortraitUrl = userPortraitUrl
                });
            }
            return result;
        }
    }
}
