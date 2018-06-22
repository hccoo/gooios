using Gooios.PaymentService.Configurations;
using Gooios.PaymentService.Proxies.DTOs;
using PaySharp.Core;
using PaySharp.Wechatpay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Proxies
{
    public interface IWeChatApiProxy
    {
        Task<WeChatOpenIdResponseDTO> CheckAuthCode(WeChatOpenIdRequestDTO model);

        Task<OrderQueryResponseDTO> GetOrder(OrderQueryRequestDTO model);
    }

    public class WeChatApiProxy : IWeChatApiProxy
    {
        readonly IServiceConfigurationProxy _configuration;

        public WeChatApiProxy(IServiceConfigurationProxy configuration)
        {
            _configuration = configuration;
        }

        public async Task<WeChatOpenIdResponseDTO> CheckAuthCode(WeChatOpenIdRequestDTO model)
        {
            WeChatOpenIdResponseDTO result = null;
            
            var api = $"https://api.weixin.qq.com/sns/jscode2session?appid={model.AppId}&secret={model.Secret}&js_code={model.Code}&grant_type={model.GrantType}";

            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(api);

                if (res.IsSuccessStatusCode)
                {
                    var resultStr = await res.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<WeChatOpenIdResponseDTO>(resultStr);
                }
            }

            //TODO:验签

            return result;
        }

        public async Task<OrderQueryResponseDTO> GetOrder(OrderQueryRequestDTO model)
        {
            OrderQueryResponseDTO result = null;
            var sign = model.ToSign(_configuration.WeChatKey);
            var api = $"https://api.mch.weixin.qq.com/pay/orderquery?appid={model.AppId}&mch_id={model.MchId}&transaction_id={model.TransactionId}&out_trade_no={model.OutTradeNo}&nonce_str={model.NonceStr}&sign={sign}&sign_type={model.SignType}";

            using (var client = new HttpClient())
            {
                var res = await client.GetAsync(api);

                if (res.IsSuccessStatusCode)
                {
                    var resultStr = await res.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<OrderQueryResponseDTO>(resultStr);
                }
            }

            //TODO:验签

            return result;
        }
    }
}
