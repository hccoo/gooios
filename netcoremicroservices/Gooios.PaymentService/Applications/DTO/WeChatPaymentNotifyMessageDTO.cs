using Gooios.Infrastructure.UtilityExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.PaymentService.Applications.DTO
{
    public class WeChatPaymentNotifyMessageDTO
    {
        [JsonProperty("return_code")]
        public string ReturnCode { get; set; }  //SUCCESS/FAIL

        [JsonProperty("return_msg")]
        public string ReturnMessage { get; set; }

        [JsonProperty("appid")]
        public string AppId { get; set; }

        [JsonProperty("mch_id")]
        public string MchId { get; set; }

        [JsonProperty("device_info")]
        public string DeviceInfo { get; set; }

        [JsonProperty("nonce_str")]
        public string NonceStr { get; set; }

        [JsonProperty("sign")]
        public string Sign { get; set; }

        [JsonProperty("sign_type")]
        public string SignType { get; set; }

        [JsonProperty("result_code")]
        public string ResultCode { get; set; }

        [JsonProperty("err_code")]
        public string ErrCode { get; set; }

        [JsonProperty("err_code_des")]
        public string ErrCodeDes { get; set; }

        [JsonProperty("openid")]
        public string OpenId { get; set; }

        [JsonProperty("is_subscribe")]
        public string IsSubscribe { get; set; }

        [JsonProperty("trade_type")]
        public string TradeType { get; set; }

        [JsonProperty("bank_type")]
        public string BankType { get; set; }

        [JsonProperty("total_fee")]
        public int TotalFee { get; set; }

        [JsonProperty("settlement_total_fee")]
        public int SettlementTotalFee { get; set; }

        [JsonProperty("fee_type")]
        public string FeeType { get; set; }

        [JsonProperty("cash_fee")]
        public int CashFee { get; set; }

        [JsonProperty("cash_fee_type")]
        public string CashFeeType { get; set; }

        [JsonProperty("coupon_fee")]
        public int CouponFee { get; set; }

        [JsonProperty("coupon_count")]
        public int CouponCount { get; set; }

        [JsonProperty("coupon_type_$n")]
        public string CouponTypeN { get; set; }

        [JsonProperty("coupon_id_$n")]
        public string CouponIdN { get; set; }

        [JsonProperty("coupon_fee_$n")]
        public int CouponFeeN { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("out_trade_no")]
        public string OutTradeNo { get; set; }

        [JsonProperty("attach")]
        public string Attach { get; set; }

        [JsonProperty("time_end")]
        public string TimeEnd { get; set; }

        public string ToSign(string key)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("appid", AppId);
            parameters.Add("mch_id", MchId);
            parameters.Add("transaction_id", TransactionId);
            parameters.Add("out_trade_no", OutTradeNo);
            parameters.Add("nonce_str", NonceStr);
            parameters.Add("return_code", ReturnCode);

            parameters.Add("return_msg", ReturnMessage);
            parameters.Add("result_code", ResultCode);
            parameters.Add("err_code", ErrCode);
            parameters.Add("err_code_des", ErrCodeDes);
            parameters.Add("device_info", DeviceInfo);
            parameters.Add("openid", OpenId);
            parameters.Add("is_subscribe", IsSubscribe);
            parameters.Add("trade_type", TradeType);
            parameters.Add("bank_type", BankType);
            parameters.Add("total_fee", TotalFee.ToString());
            parameters.Add("settlement_total_fee", SettlementTotalFee.ToString());
            parameters.Add("fee_type", FeeType);
            parameters.Add("cash_fee", CashFee.ToString());
            parameters.Add("cash_fee_type", CashFeeType);
            parameters.Add("coupon_fee", CouponFee.ToString());
            parameters.Add("coupon_count", CouponCount.ToString());
            parameters.Add("coupon_type_$n", CouponTypeN);
            parameters.Add("coupon_id_$n", CouponIdN);
            parameters.Add("coupon_fee_$n", CouponFeeN.ToString());
            parameters.Add("attach", Attach);
            parameters.Add("time_end", TimeEnd);

            parameters.OrderBy(o => o.Key);

            var sg = "";
            foreach (var item in parameters)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    sg = string.Join('&', sg, string.Join('=', item.Key, item.Value));
                }
            }
            var signValue = $"{sg}&key={key}".ToMD5().ToUpper();

            return signValue;
        }
    }
}
