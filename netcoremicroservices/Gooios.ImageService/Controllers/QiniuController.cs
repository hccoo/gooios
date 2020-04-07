using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Qiniu.Storage;
using Qiniu.Util;

namespace Gooios.ImageService.Controllers
{
    [Produces("application/json")]
    [Route("api/Qiniu")]
    public class QiniuController : Controller
    {
        [Route("config")]
        public QiniuConfig Get(string fileName)
        {
            Request.Headers.TryGetValue("qiniukey", out StringValues vals);
            var qiniuKey = vals.FirstOrDefault();
            if (string.IsNullOrEmpty(qiniuKey)) return null;
            if (qiniuKey != "cookBcd2020") return null;

            var accessKey = "72vGWO8zK9EqGIK_-qUFRjG1JIogApCV13ls57Dv";
            var secretKey = "iuxEsdQovi7ltkYpBIMcF8-Q7arXS8S0zUP8-Wzx";
            var bucket = "gooioscook";

            var mac = new Mac(accessKey, secretKey);

            //var rand = new Random();
            //string key = string.Format("UploadFileCookApp_{0}.dat", rand.Next());
            //string filePath = LocalFile;
            string suffix = fileName.Split('.')[fileName.Split('.').Length - 1];
            string name = Guid.NewGuid().ToString().Replace("-", "");
            string key = $"cookapp_{name}.{suffix}";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket + ":" + key;
            //putPolicy.SetExpires(3600);
            //putPolicy.DeleteAfterDays = 100000;
            var uploadToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            return new QiniuConfig
            {
                Key = key,
                UploadToken = uploadToken,
                FileUrl = $"http://q85ws0856.bkt.clouddn.com/{key}"
            };
        }
    }

    public class QiniuConfig
    {
        //public string AccessToken { get; set; }

        //public string SecretToken { get; set; }

        //public string Bucket { get; set; }

        public string UploadToken { get; set; }

        public string FileUrl { get; set; }

        public string Key { get; set; }
    }
}