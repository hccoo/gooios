using Gooios.UserService.Configurations;
using Gooios.UserService.Domain.Aggregates;
using Gooios.UserService.Domain.Repositories;
using Gooios.UserService.Proxies;
using Gooios.Infrastructure;
using Gooios.Infrastructure.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Gooios.UserService.Applications.Dtos;

namespace Gooios.UserService.Application.Services
{
    public interface IUserAppService : IApplicationServiceContract
    {
        CookAppUserDto VerifyCookAppUserByPassword(string userName, string password);

        Task<CookAppUserDto> VerifyCookAppUserByVerifyCode(string userName, string code);

        Task<CookAppPartnerLoginUserDto> VerifyCookAppPartnerLoginUserByAuthCode(string authCode);

        CookAppUserDto AddCookAppUser(string userName, string password, string mobile, string email);

        bool SetServicerIdForUser(string userName, string servicerId);

        CookAppUserDto GetUser(string userName);
    }

    public class UserAppService : ApplicationServiceContract, IUserAppService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ICookAppUserRepository _cookappUserRepo;
        private readonly ICookAppPartnerLoginUserRepository _cookappPartnerLoginUserRepo;
        private readonly IVerificationProxy _verificationProxy;
        private readonly IWechatProxy _wechatProxy;
        private readonly IServiceConfigurationAgent _config;

        public UserAppService(ICookAppUserRepository cookappUserRepo,
            ICookAppPartnerLoginUserRepository cookappPartnerLoginUserRepo,
            IVerificationProxy verificationProxy,
            IWechatProxy wechatProxy,
            IServiceConfigurationAgent config,
            IDbUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _cookappUserRepo = cookappUserRepo;
            _cookappPartnerLoginUserRepo = cookappPartnerLoginUserRepo;
            _verificationProxy = verificationProxy;
            _wechatProxy = wechatProxy;
            _config = config;
        }

        public async Task<CookAppPartnerLoginUserDto> VerifyCookAppPartnerLoginUserByAuthCode(string authCode)
        {
            var ret = await _wechatProxy.GetAccessToken(_config.WeChatAppId, _config.WeChatAppSecret, authCode, "authorization_code");
            if (ret == null) return null;

            var partnerUser = CookAppPartnerLoginUserFactory.CreateInstance(authCode, ret.AccessToken, ret.ExpiresIn, ret.RefreshToken, ret.Scope, ret.UnionId, LoginChannel.Wechat, ret.OpenId);
            _cookappPartnerLoginUserRepo.Add(partnerUser);
            _dbUnitOfWork.Commit();

            return MapperProvider.Mapper.Map<CookAppPartnerLoginUserDto>(partnerUser);
        }

        public CookAppUserDto VerifyCookAppUserByPassword(string userName, string password)
        {
            var obj = _cookappUserRepo.GetFiltered(o => o.UserName == userName && o.Password == SecretProvider.EncryptToMD5(password)).FirstOrDefault();
            if (obj == null) return null;
            return MapperProvider.Mapper.Map<CookAppUserDto>(obj);
        }

        public async Task<CookAppUserDto> VerifyCookAppUserByVerifyCode(string userName, string code)
        {
            var obj = _cookappUserRepo.GetFiltered(o => o.UserName == userName).FirstOrDefault();

            //_verificationProxy.Test(1);
            if (obj != null)
            {
                var verification = await _verificationProxy.GetVerification(BizCode.Login, userName);
                if (verification == null) return null;
                if (verification.Code != code) return null;

                await _verificationProxy.SetVerificationUsed(verification);
                ;
                return MapperProvider.Mapper.Map<CookAppUserDto>(obj);
            }
            else
            {
                var verification = await _verificationProxy.GetVerification(BizCode.Login, userName);
                if (verification == null) return null;
                if (verification.Code != code) return null;

                var user = CookAppUserFactory.CreateInstance(userName, Guid.NewGuid().ToString().Substring(0, 8), userName, "");
                _cookappUserRepo.Add(user);
                _dbUnitOfWork.Commit();

                await _verificationProxy.SetVerificationUsed(verification);

                return MapperProvider.Mapper.Map<CookAppUserDto>(user);
            }

        }

        public CookAppUserDto AddCookAppUser(string userName, string password, string mobile, string email)
        {
            //TODO:verify the input data
            var user = CookAppUserFactory.CreateInstance(userName, password, mobile, email);
            _cookappUserRepo.Add(user);
            _dbUnitOfWork.Commit();
            return MapperProvider.Mapper.Map<CookAppUserDto>(user);
        }

        public bool SetServicerIdForUser(string userName, string servicerId)
        {
            var user = _cookappUserRepo.GetFiltered(o => o.UserName == userName).FirstOrDefault();
            user.ServicerId = servicerId;
            _cookappUserRepo.Update(user);
            _dbUnitOfWork.Commit();
            return true;
        }

        public CookAppUserDto GetUser(string idOrUserName)
        {
            var user = _cookappUserRepo.GetFiltered(o => o.Id == idOrUserName).FirstOrDefault();
            if (user == null)
            {
                user = _cookappUserRepo.GetFiltered(o => o.UserName == idOrUserName).FirstOrDefault();
            }
            if (user == null) return null;

            return new CookAppUserDto
            {
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
                Email = user.Email,
                Id = user.Id,
                Mobile = user.Mobile,
                UserName = user.UserName,
                ServicerId = user.ServicerId,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn
            };
        }
    }
}
