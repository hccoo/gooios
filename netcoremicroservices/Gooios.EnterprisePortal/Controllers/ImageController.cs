using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Gooios.EnterprisePortal.Utilities;
using Gooios.EnterprisePortal.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Gooios.EnterprisePortal.Models;
using Gooios.EnterprisePortal.Services;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using Qiniu.Storage;
using Qiniu.Http;
using Qiniu.Util;

namespace Gooios.EnterprisePortal.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        private IHostingEnvironment hostingEnv;

        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };

        public ImageController(IHostingEnvironment env,
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              IEmailSender emailSender,
              ILogger<ManageController> logger,
              UrlEncoder urlEncoder)
        {
            this.hostingEnv = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 20MB refuse upload !
            if (size > 2048000) return Json(new { errno = 3, data = new string[] { } });

            var result = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string suffix = fileName.Split('.')[1];

                if (!pictureFormatArray.Contains(suffix))
                {
                    //图片格式不对，必须为"png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO"
                    return Json(new { errno = 4, data = new string[] { } });
                }

                var stream = file.OpenReadStream();

                byte[] imgArr = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(imgArr, 0, (int)stream.Length);
                stream.Close();

                var imgBase64Content = Convert.ToBase64String(imgArr);

                var img = new ImageModel
                {
                    CreatedBy = applicationUser.Id,
                    Description = string.Empty,
                    Title = string.Empty,
                    ImageBase64Content = imgBase64Content,
                    ClientFileName = fileName.Split('.')[0]
                };

                var imgApiUrl = "https://imageservice.gooios.com/api/image/v1";
                var jsonObj = JsonConvert.SerializeObject(img);
                var res = await HttpRequestHelper.PostAsync<ImageModel>(imgApiUrl, jsonObj, "63e960bff18111e799160126c7e9f004",applicationUser.Id);
                res.ClientFileName = img.ClientFileName;
                result.Add(res.HttpPath);
            }

            return Json(new { errno = 0, data = result.ToArray() });
        }

        [HttpPost]
        public async Task<IActionResult> DiyUpload()
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 20MB refuse upload !
            if (size > 2048000) return Json(new { errno = 3, data = new string[] { } });

            var result = new List<ImageModel>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string suffix = fileName.Split('.')[1];

                if (!pictureFormatArray.Contains(suffix))
                {
                    //图片格式不对，必须为"png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO"
                    return Json(new { errno = 4, data = new string[] { } });
                }

                var stream = file.OpenReadStream();

                byte[] imgArr = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(imgArr, 0, (int)stream.Length);
                stream.Close();

                var imgBase64Content = Convert.ToBase64String(imgArr);

                var img = new ImageModel
                {
                    CreatedBy = applicationUser.Id,
                    Description = string.Empty,
                    Title = string.Empty,
                    ImageBase64Content = imgBase64Content,
                    ClientFileName = fileName.Split('.')[0]
                };

                var imgApiUrl = "https://imageservice.gooios.com/api/image/v1";
                var jsonObj = JsonConvert.SerializeObject(img);
                var res = await HttpRequestHelper.PostAsync<ImageModel>(imgApiUrl, jsonObj, "63e960bff18111e799160126c7e9f004", applicationUser.Id);
                res.ClientFileName = img.ClientFileName;
                result.Add(res);
            }

            return Json(new { errno = 0, data = result.ToArray() });
        }


        [HttpPost]
        public async Task<IActionResult> QiniuUpload()
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 20MB refuse upload !
            if (size > 2048000) return Json(new { errno = 3, data = new string[] { } });

            var result = new List<ImageModel>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string suffix = fileName.Split('.')[1];

                if (!pictureFormatArray.Contains(suffix))
                {
                    //图片格式不对，必须为"png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO"
                    return Json(new { errno = 4, data = new string[] { } });
                }

                var stream = file.OpenReadStream();

                byte[] imgArr = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(imgArr, 0, (int)stream.Length);
                stream.Close();

                var imgBase64Content = Convert.ToBase64String(imgArr);

                var img = new ImageModel
                {
                    CreatedBy = applicationUser.Id,
                    Description = string.Empty,
                    Title = string.Empty,
                    ImageBase64Content = "",//imgBase64Content,
                    ClientFileName = fileName.Split('.')[0]
                };



                var accessKey = "72vGWO8zK9EqGIK_-qUFRjG1JIogApCV13ls57Dv";
                var secretKey = "iuxEsdQovi7ltkYpBIMcF8-Q7arXS8S0zUP8-Wzx";
                var bucket = "gooioscook";
                var mac = new Mac(accessKey, secretKey);
                string sfx = fileName.Split('.')[fileName.Split('.').Length - 1];
                string name = Guid.NewGuid().ToString().Replace("-", "");
                string key = $"cookapp_{name}.{sfx}";
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = bucket + ":" + key;
                var uploadToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

                var config = new Config();
                config.Zone = Zone.ZoneCnEast;
                config.UseHttps = true;
                config.UseCdnDomains = true;
                config.ChunkSize = ChunkUnit.U512K;
                var target = new FormUploader(config);
                var upload_token = uploadToken;
                HttpResult updresult = target.UploadFile(file.FileName, key, upload_token, null).ConfigureAwait(false).GetAwaiter().GetResult();
                img.HttpPath = $"http://q85ws0856.bkt.clouddn.com/{key}";


                var imgApiUrl = "https://imageservice.gooios.com/api/image/v1";
                var jsonObj = JsonConvert.SerializeObject(img);
                var res = await HttpRequestHelper.PostAsync<ImageModel>(imgApiUrl, jsonObj, "63e960bff18111e799160126c7e9f004", applicationUser.Id);
                res.ClientFileName = img.ClientFileName;
                result.Add(res);
            }

            return Json(new { errno = 0, data = result.ToArray() });
        }

    }
}