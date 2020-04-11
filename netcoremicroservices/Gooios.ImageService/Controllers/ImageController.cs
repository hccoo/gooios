using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.ImageService.Applications.Services;
using Gooios.ImageService.Applications.DTO;
using Microsoft.Extensions.Primitives;

namespace Gooios.ImageService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/v1")]
    public class ImageController : Controller
    {
        readonly IImageAppService _imageAppService;

        public ImageController(IImageAppService imageAppService)
        {
            _imageAppService = imageAppService;
        }

        [HttpGet("{id}")]
        public ImageDTO Get(string id)
        {
            return _imageAppService.GetImage(id);
        }

        [Route("images")]
        [HttpGet("{ids}")]
        public IEnumerable<ImageDTO> GetImages(string ids)
        {
            var idList = ids.Split(',').ToList();
            return _imageAppService.GetImages(idList);
        }

        [Route("resource")]
        public AppletResource GetResource()
        {
            return new AppletResource
            {
                ADBanner = new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_04.jpg", Url = "", ItemType="goods", ItemKey = "81b6656a-6408-4165-b5e6-acd9b4a63b6f" },
                IndexBanners = new List<ADItem> {
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_01.jpg", Url="https://www.arcanestars.com/topics/testtopic/index.html", ItemType="topic", ItemKey="" },
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_02.jpg", Url="", ItemType="goods", ItemKey="09e44830-b44f-498d-9d98-4072975800a3" },
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_03.jpg", Url="https://www.arcanestars.com/topics/testtopic/index.html", ItemType="topic", ItemKey="" }
                },
                GrouponBanners = new List<ADItem> {
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_05.jpg", Url="https://www.arcanestars.com/topics/testtopic/index.html", ItemType="topic", ItemKey="" },
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_06.jpg", Url="", ItemType="goods", ItemKey="68db599a-64f3-47e2-9083-50f98a14081c" },
                    new ADItem { Image = $"https://{this.HttpContext.Request.Host}/UploadImages/resources/banner_07.jpg", Url="https://www.arcanestars.com/topics/testtopic/index.html", ItemType="topic", ItemKey="" }
                }
            };
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ImageDTO model)
        {
            var host = Request.Host.Value;
            var protocol = Request.IsHttps ? "https://" : "http://";
            var userId = Request.Headers["userId"].FirstOrDefault();

            if(!string.IsNullOrEmpty(model.ImageBase64Content))
                model.HttpPath = $"{protocol}{host}/uploadimages/";

            model.CreatedBy = userId;
            var result = _imageAppService.AddImage(model);

            return new OkObjectResult(result);
        }
    }

    public class AppletResource
    {
        public List<ADItem> IndexBanners { get; set; }

        public List<ADItem> GrouponBanners { get; set; }

        public ADItem ADBanner { get; set; }
    }

    public class ADItem
    {
        public string Image { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// topic/goods/service/...
        /// </summary>
        public string ItemType { get; set; }

        public string ItemKey { get; set; }
    }
}