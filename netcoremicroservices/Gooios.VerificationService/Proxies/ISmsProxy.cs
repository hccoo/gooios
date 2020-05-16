using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Gooios.VerificationService.Proxies
{
    public interface ISmsProxy
    {
        Task SendVerificationCode(string param, string phone, string templateId);
    }

    /// <summary>
    /// 注册模板：33592
    /// 忘记密码模板：33593
    /// </summary>
    public class FegineSmsProxy : ISmsProxy
    {
        const string apiUrl = "https://feginesms.market.alicloudapi.com/codeNotice";
        const string appCode = "0dc41b203b32448d886495b08f8090ef";
        const string signContent = "500467";//"38358";
        //const string templateId = "";

        public FegineSmsProxy()
        {

        }

        public async Task SendVerificationCode(string param, string phone,string templateId)
        {
            try
            {
                string sign = signContent;
                string skin = templateId;
                var apiUri = $"{apiUrl}?param={param}&phone={phone}&sign={sign}&skin={skin}";
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                using (var client = new HttpClient())
                {
                    var req = new HttpRequestMessage(HttpMethod.Get, apiUri);
                    client.DefaultRequestHeaders.Add("Authorization", "APPCODE " + appCode);
                    var res = await client.SendAsync(req);
                }
            }
            catch(Exception ex)
            {
                //TODO: process
            }
        }

        bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
