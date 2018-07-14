using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gooios.FancyService.Applications.DTOs;
using Gooios.FancyService.Applications.Services;

namespace Gooios.FancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/category/v1")]
    public class CategoryController : BaseApiController
    {
        readonly ICategoryAppService _categoryAppService;
        public CategoryController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        [HttpPost]
        public void Post([FromBody]CategoryDTO model)
        {
            _categoryAppService.AddServiceCategory(model, UserId ?? "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mark">Service or Servicer</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbymark")]
        public IEnumerable<CategoryDTO> GetByMark(string mark, string appId = "")
        {
            return _categoryAppService.GetCategoriesByMark(mark, appId);
        }

        [HttpGet]
        [Route("getbyparentid")]
        public IEnumerable<CategoryDTO> GetByParentId(string id)
        {
            return _categoryAppService.GetCategoriesByParentId(id);
        }
    }
}