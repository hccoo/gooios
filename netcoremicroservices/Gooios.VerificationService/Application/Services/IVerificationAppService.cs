using Gooios.Infrastructure;
using Gooios.Infrastructure.Events;
using Gooios.Infrastructure.Transactions;
using Gooios.VerificationService.Application.DTO;
using Gooios.VerificationService.Domain.Aggregates;
using Gooios.VerificationService.Domain.Repositories;
using Gooios.VerificationService.Domain.Services;
using Gooios.VerificationService.Proxies;
using System;
using System.Linq;

namespace Gooios.VerificationService.Application.Services
{
    public interface IVerificationAppService : IApplicationServiceContract
    {
        void AddVerification(VerificationDTO verificationDTO);

        VerificationDTO GetAvailableVerification(BizCode bizCode, string to);

        void SetVerificationUsed(VerificationDTO verificationDTO);
    }

    class VerificationAppService : ApplicationServiceContract, IVerificationAppService
    {
        readonly IVerificationRepository _verificationRepository;
        readonly IVerificationService _verificationService;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IEventBus _eventBus;
        readonly ISmsProxy _smsProxy;

        public VerificationAppService(
            IVerificationRepository verificationRepository,
            IVerificationService verificationService,
            IDbUnitOfWork dbUnitOfWork,
            IEventBus eventBus,
            ISmsProxy smsProxy)
        {
            _verificationRepository = verificationRepository;
            _verificationService = verificationService;
            _dbUnitOfWork = dbUnitOfWork;
            _eventBus = eventBus;
            _smsProxy = smsProxy;
        }

        public void AddVerification(VerificationDTO verificationDTO)
        {
            using (ITransactionCoordinator coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
            {
                var verifications = _verificationRepository.GetFiltered(o => o.IsSuspend == false && o.To == verificationDTO.To && o.BizCode == verificationDTO.BizCode).ToList();

                var lastRecord = verifications.OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                if (lastRecord != null)
                {
                    if (lastRecord.CreatedOn.AddSeconds(60) > DateTime.Now)
                    {
                        return;
                    }
                }

                _verificationService.SetVerificationsSuspend(verifications);

                var verification = VerificationFactory.CreateVerification(verificationDTO.BizCode, verificationDTO.To);
                verification.CreatedConfirm();
                _verificationRepository.Add(verification);

                coordinator.Commit();
            }
        }

        public void SetVerificationUsed(VerificationDTO verificationDTO)
        {
            var verification = _verificationRepository.GetFiltered(o => o.To == verificationDTO.To && o.BizCode == verificationDTO.BizCode && o.Code == verificationDTO.Code && o.IsUsed == false && o.IsSuspend == false).OrderByDescending(g => g.CreatedOn).FirstOrDefault();

            if (verification != null)
            {
                verification.SetUsed();
                _dbUnitOfWork.Commit();
            }
        }

        public VerificationDTO GetAvailableVerification(BizCode bizCode, string to)
        {
            var now = DateTime.Now;
            var result = _verificationRepository.GetFiltered(o =>
                            o.BizCode == bizCode
                            && to == o.To
                            && o.IsSuspend == false
                            && o.IsUsed == false
                            && o.ExpiredOn > now).OrderByDescending(g => g.CreatedOn).FirstOrDefault();

            return result == null ? null : new VerificationDTO { BizCode = result.BizCode, Code = result.Code, To = result.To };
        }
    }
}
