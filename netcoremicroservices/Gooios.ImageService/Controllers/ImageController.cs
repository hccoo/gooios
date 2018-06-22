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

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ImageDTO model)
        {
            var host = Request.Host.Value;
            var protocol = Request.IsHttps ? "https://" : "http://";
            var userId = Request.Headers["userId"].FirstOrDefault();

            model.HttpPath = $"{protocol}{host}/uploadimages/";
            model.CreatedBy = userId;
            var result = _imageAppService.AddImage(model);

            return new OkObjectResult(result);
        }
    }
}