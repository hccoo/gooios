using Gooios.Infrastructure;
using Gooios.PaymentService.Applications.DTO;
using Gooios.PaymentService.Configurations;
using Gooios.PaymentService.Domains.Aggregates;
using Gooios.PaymentService.Domains.Repositories;
using Gooios.PaymentService.Proxies;
using Gooios.PaymentService.Proxies.DTOs;
using PaySharp.Core;
using PaySharp.Wechatpay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PaySharp.Wechatpay.Request;
using PaySharp.Wechatpay.Domain;
using System.Security.Cryptography;
using System.Text;

namespace Gooios.PaymentService.Applications.Services
{
    public interface IWeChatPaymentAppService : IApplicationServiceContract
    {
        Task<RequestPaymentResponseDTO> RequestPayment(RequestPaymentRequestDTO model);

        Task<string> GetOpenId(string code, string organizationId = null);

        Task<OpenIDSessionKeyDTO> GetSessionKey(string code, string organizationId = null, string applicationId = null, string nickName = "", string portraitUrl = "");

        Task SetPaidSuccessed(string orderId);

        Task SetPaidFailed(string orderId);

        Task SyncOrderStatus(string orderId);
    }
    public class WeChatPaymentAppService : ApplicationServiceContract, IWeChatPaymentAppService
    {
        readonly IGateways _gateways;
        readonly IServiceConfigurationProxy _configuration;
        readonly IWeChatApiProxy _wechatApiProxy;
        readonly IOrderServiceProxy _orderServiceProxy;
        readonly IWeChatAppConfigurationRepository _wechatAppConfigurationRepository;
        readonly IAuthServiceProxy _authServiceProxy;

        public WeChatPaymentAppService(IGateways gateways, IServiceConfigurationProxy configuration, IWeChatApiProxy wechatApiProxy, IOrderServiceProxy orderServiceProxy, IWeChatAppConfigurationRepository wechatAppConfigurationRepository, IAuthServiceProxy authServiceProxy)
        {
            _gateways = gateways;
            _configuration = configuration;
            _wechatApiProxy = wechatApiProxy;
            _orderServiceProxy = orderServiceProxy;
            _wechatAppConfigurationRepository = wechatAppConfigurationRepository;
            _authServiceProxy = authServiceProxy;
        }

        public async Task<string> GetOpenId(string code, string organizationId = null)
        {
            WeChatOpenIdResponseDTO result = null;

            BaseGateway gateway = null;

            if (string.IsNullOrEmpty(organizationId))
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }
            else
            {
                WeChatAppConfiguration appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == organizationId).FirstOrDefault();
                if (appConfig != null)
                {
                    var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                    {
                        AppId = appConfig.AppId,
                        MchId = appConfig.MchId,
                        Key = appConfig.Key,
                        AppSecret = appConfig.AppSecret,
                        SslCertPath = appConfig.SslCertPath,
                        SslCertPassword = appConfig.SslCertPassword,
                        NotifyUrl = appConfig.NotifyUrl
                    };

                    gateway = new WechatpayGateway(wechatpayMerchant);
                }
                else
                {
                    gateway = _gateways.Get<WechatpayGateway>();
                }
            }

            var reqModel = new WeChatOpenIdRequestDTO { AppId = gateway.Merchant.AppId, Code = code, Secret = _configuration.WeChatAppSecret };

            result = await _wechatApiProxy.CheckAuthCode(reqModel);

            //TODO:验签

            return result?.OpenId ?? string.Empty;
        }

        public async Task<OpenIDSessionKeyDTO> GetSessionKey(string code, string organizationId = null, string applicationId = null, string nickName = "", string portraitUrl = "")
        {
            WeChatOpenIdResponseDTO result = null;

            BaseGateway gateway = null;

            if (string.IsNullOrEmpty(organizationId))
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }
            else
            {
                WeChatAppConfiguration appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == organizationId).FirstOrDefault();
                if (appConfig != null)
                {
                    var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                    {
                        AppId = appConfig.AppId,
                        MchId = appConfig.MchId,
                        Key = appConfig.Key,
                        AppSecret = appConfig.AppSecret,
                        SslCertPath = appConfig.SslCertPath,
                        SslCertPassword = appConfig.SslCertPassword,
                        NotifyUrl = appConfig.NotifyUrl
                    };

                    gateway = new WechatpayGateway(wechatpayMerchant);
                }
                else
                {
                    gateway = _gateways.Get<WechatpayGateway>();
                }
            }

            var reqModel = new WeChatOpenIdRequestDTO { AppId = gateway.Merchant.AppId, Code = code, Secret = _configuration.WeChatAppSecret };

            result = await _wechatApiProxy.CheckAuthCode(reqModel);

            //TODO:验签

            if (result.OpenId != null)
            {
                AppletUserSessionDTO dto = null;
                if (result != null)
                {
                    await _authServiceProxy.AddOrUpdateAppletUser(new AppletUserDTO
                    {
                        ApplicationId = applicationId,
                        Channel = UserChannel.WeChat,
                        NickName = nickName,
                        OpenId = result.OpenId,
                        OrganizationId = organizationId,
                        UserId = result.OpenId,
                        UserPortrait = portraitUrl
                    });

                    dto = await _authServiceProxy.AddOrUpdateAppletUserSession(new AppletUserSessionDTO
                    {
                        UserId = result.OpenId,
                        OpenId = result.OpenId,
                        SessionKey = result.SessionKey
                    });
                }

                return new OpenIDSessionKeyDTO { GooiosSessionKey = dto?.GooiosSessionKey ?? "", OpenId = dto?.OpenId ?? "", SessionKey = result.SessionKey };
            }
            else
                return null;
        }

        public async Task<RequestPaymentResponseDTO> RequestPayment(RequestPaymentRequestDTO model)
        {
            if (model.PayChannel == PayChannel.AppletPay)
                return await AppletPay(model);

            if (model.PayChannel == PayChannel.AppPay)
                return await AppPay(model);

            return null;
        }

        #region request pay logic

        async Task<RequestPaymentResponseDTO> AppletPay(RequestPaymentRequestDTO model)
        {
            var orderDTO = await _orderServiceProxy.GetById(model.OrderId);

            if (orderDTO == null) return null;

            var request = new AppletPayRequest();

            var body = "";
            var orgStr = (orderDTO.OrderItems.FirstOrDefault()?.Title ?? "-");
            if (orgStr.Length > 20)
                body = orgStr.Substring(0, 20);
            else
                body = orgStr;

            request.AddGatewayData(new AppletPayModel()
            {
                Body = body,
                OutTradeNo = orderDTO.OrderNo,
                TotalAmount = (int)(orderDTO.PayAmount * 100),
                OpenId = model.OpenId
            });

            IGateway gateway = null;

            var organizationId = "";

            organizationId = orderDTO.OrganizationId ?? "";
            WeChatAppConfiguration appConfig = null;
            if (!string.IsNullOrEmpty(organizationId))
            {
                appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == organizationId).FirstOrDefault();
            }

            if (appConfig != null)
            {
                var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                {
                    AppId = appConfig.AppId,
                    MchId = appConfig.MchId,
                    Key = appConfig.Key,
                    AppSecret = appConfig.AppSecret,
                    SslCertPath = appConfig.SslCertPath,
                    SslCertPassword = appConfig.SslCertPassword,
                    NotifyUrl = appConfig.NotifyUrl
                };

                gateway = new WechatpayGateway(wechatpayMerchant);
            }
            else
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }

            var response = gateway.Execute(request);

            var ns = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var ts = GetTimeStamp();
            var pkg = $"prepay_id={response?.PrepayId}";
            var sign = GetSign(((WechatpayGateway)gateway).Merchant.AppId, ns, response?.PrepayId, ts.ToString(), ((WechatpayGateway)gateway).Merchant.Key);

            var result = new RequestPaymentResponseDTO
            {
                NonceStr = ns,
                Package = pkg,
                PaySign = sign,
                TimeStamp = ts.ToString()
            };

            return result;
        }

        async Task<RequestPaymentResponseDTO> AppPay(RequestPaymentRequestDTO model)
        {
            var orderDTO = await _orderServiceProxy.GetById(model.OrderId);

            if (orderDTO == null) return null;

            var request = new AppPayRequest();

            var body = "";
            var orgStr = (orderDTO.OrderItems.FirstOrDefault()?.Title ?? "-");
            if (orgStr.Length > 20)
                body = orgStr.Substring(0, 20);
            else
                body = orgStr;

            request.AddGatewayData(new AppPayModel()
            {
                Body = body,
                OutTradeNo = orderDTO.OrderNo,
                TotalAmount = (int)(orderDTO.PayAmount * 100)
            });

            IGateway gateway = null;

            var organizationId = "";

            organizationId = orderDTO.OrganizationId ?? "";
            WeChatAppConfiguration appConfig = null;
            if (!string.IsNullOrEmpty(organizationId))
            {
                appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == organizationId).FirstOrDefault();
            }

            if (appConfig != null)
            {
                var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                {
                    AppId = appConfig.AppId,
                    MchId = appConfig.MchId,
                    Key = appConfig.Key,
                    AppSecret = appConfig.AppSecret,
                    SslCertPath = appConfig.SslCertPath,
                    SslCertPassword = appConfig.SslCertPassword,
                    NotifyUrl = appConfig.NotifyUrl
                };

                gateway = new WechatpayGateway(wechatpayMerchant);
            }
            else
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }

            var response = gateway.Execute(request);

            var ns = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var ts = GetTimeStamp();
            var pkg = $"prepay_id={response?.PrepayId}";
            var sign = GetSign(((WechatpayGateway)gateway).Merchant.AppId, ns, response?.PrepayId, ts.ToString(), ((WechatpayGateway)gateway).Merchant.Key);

            var result = new RequestPaymentResponseDTO
            {
                NonceStr = ns,
                Package = pkg,
                PaySign = sign,
                TimeStamp = ts.ToString()
            };

            return result;
        }

        #endregion

        public async Task SetPaidSuccessed(string orderId)
        {
            await _orderServiceProxy.SetOrderPaidSuccessed(orderId);
        }

        public async Task SetPaidFailed(string orderId)
        {
            await _orderServiceProxy.SetOrderPaidFailed(orderId);
        }

        public async Task SyncOrderStatus(string orderId)
        {
            var orderDTO = await _orderServiceProxy.GetById(orderId);

            if (orderDTO == null) return;

            BaseGateway gateway = null;

            if (string.IsNullOrEmpty(orderDTO.OrganizationId))
            {
                gateway = _gateways.Get<WechatpayGateway>();
            }
            else
            {
                WeChatAppConfiguration appConfig = _wechatAppConfigurationRepository.GetFiltered(o => o.OrganizationId == orderDTO.OrganizationId).FirstOrDefault();
                if (appConfig != null)
                {
                    var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                    {
                        AppId = appConfig.AppId,
                        MchId = appConfig.MchId,
                        Key = appConfig.Key,
                        AppSecret = appConfig.AppSecret,
                        SslCertPath = appConfig.SslCertPath,
                        SslCertPassword = appConfig.SslCertPassword,
                        NotifyUrl = appConfig.NotifyUrl
                    };

                    gateway = new WechatpayGateway(wechatpayMerchant);
                }
                else
                {
                    gateway = _gateways.Get<WechatpayGateway>();
                }
            }


            var request = new QueryRequest();
            request.AddGatewayData(new QueryModel()
            {
                OutTradeNo = orderDTO.OrderNo
            });

            var response = gateway.Execute(request);

            if (response.ReturnCode == "SUCCESS")
            {
                if (response.ResultCode == "SUCCESS")
                {
                    if (response.TradeState == TradeState.SUCCESS.ToString())
                    {
                        await _orderServiceProxy.SetOrderPaidSuccessed(orderId);
                    }
                    if (response.TradeState == TradeState.REVOKED.ToString()
                        || response.TradeState == TradeState.CLOSED.ToString())
                    {
                        await _orderServiceProxy.SetOrderPaidFailed(orderId);
                    }
                }
            }
        }

        public async Task<OrderQueryResponseDTO> QueryOrder(OrderQueryRequestDTO model)
        {
            return await _wechatApiProxy.GetOrder(model);
        }

        string GetSign(string appId, string nonceStr, string prepayId, string timeStamp, string key)
        {
            var input = $"appId={appId}&nonceStr={nonceStr}&package=prepay_id={prepayId}&signType=MD5&timeStamp={timeStamp}&key={key}";

            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        long GetTimeStamp()
        {
            TimeSpan cha = (DateTime.Now - TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1)));
            long t = (long)cha.TotalSeconds;
            return t;
        }
    }

    public enum TradeState
    {
        SUCCESS,        //—支付成功
        REFUND,         //—转入退款
        NOTPAY,         //—未支付
        CLOSED,         //—已关闭
        REVOKED,        //—已撤销（刷卡支付）
        USERPAYING,     //--用户支付中
        PAYERROR,       //--支付失败(其他原因，如银行返回失败)
    }

}
