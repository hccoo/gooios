using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Gooios.ImageService.Controllers
{
    [Produces("application/json")]
    [Route("api/Qiniu")]
    public class QiniuController : Controller
    {
        [Route("config")]
        public QiniuConfig Get()
        {
            Request.Headers.TryGetValue("qiniukey", out StringValues vals);
            var key = vals.FirstOrDefault();
            if (string.IsNullOrEmpty(key)) return null;
            if (key != "cookBcd2020") return null;

            return new QiniuConfig
            {
                AccessToken = "72vGWO8zK9EqGIK_-qUFRjG1JIogApCV13ls57Dv",
                SecretToken = "iuxEsdQovi7ltkYpBIMcF8-Q7arXS8S0zUP8-Wzx",
                Bucket = "gooioscook"
            };
        }
    }

    public class QiniuConfig
    {
        public string AccessToken { get; set; }

        public string SecretToken { get; set; }

        public string Bucket { get; set; }
    }
}