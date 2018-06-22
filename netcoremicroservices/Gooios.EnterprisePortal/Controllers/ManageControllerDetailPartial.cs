using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gooios.EnterprisePortal.Utilities;
using Gooios.EnterprisePortal.Models.ManageViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Gooios.EnterprisePortal.Controllers
{
    public partial class ManageController
    {
        [Authorize(Roles = "GoodsOwner")]
        [HttpGet]
        public async Task<IActionResult> GoodsDetail(string id = null)
        {
            ViewBag.Operation = string.IsNullOrEmpty(id?.Trim()) ? "add" : "update";
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var categories = await HttpRequestHelper.GetAsync<List<GoodsCategoryModel>>("http://goodsservice.gooios.com/api/goodscategory/v1", "", "63e960bff18111e79916012cc8e9f005", applicationUser.Id);

            var goodsCategories = categories.Where(o => string.IsNullOrEmpty(o.ParentId)).Select(item =>
            {
                return new GoodsCategoryModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Children = categories.Where(g => g.ParentId == item.Id).ToList()
                };
            }).ToList();
            ViewBag.Categories = goodsCategories;
            ViewBag.CategoriesJson = JsonConvert.SerializeObject(goodsCategories);

            GoodsModel model = string.IsNullOrEmpty(id) ? null : await HttpRequestHelper.GetAsync<GoodsModel>($"http://goodsservice.gooios.com/api/goods/v1/getgoods", $"id={id}", "63e960bff18111e79916012cc8e9f005", applicationUser.Id);

            return View(model);
        }

        [Authorize(Roles = "GoodsOwner")]
        [HttpPost]
        public async Task<IActionResult> GoodsDetail(GoodsModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });

            model.SubCategory = (string.IsNullOrEmpty(model.SubCategory) || model.SubCategory == "请选择") ? null : model.SubCategory;

            var url = "http://goodsservice.gooios.com/api/goods/v1";
            var ourl = "http://organizationservice.gooios.com/api/organization/v1/getbyid";
            if (string.IsNullOrEmpty(model.Id))
            {
                model.StoreId = applicationUser.OrganizationId;
                model.OrganizationId = applicationUser.OrganizationId;

                var org = await HttpRequestHelper.GetAsync<OrganizationDTO>(ourl, $"id={applicationUser.OrganizationId}", "999960bff18111e799160126c7e9f568", applicationUser.Id);

                if (org != null)
                {
                    model.Address = org.Address;
                }
                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PostNoResultAsync(url, jsonObj, "63e960bff18111e79916012cc8e9f005", applicationUser.Id);
            }
            else
            {
                model.StoreId = applicationUser.OrganizationId;
                model.OrganizationId = applicationUser.OrganizationId;

                var org = await HttpRequestHelper.GetAsync<OrganizationDTO>(ourl, $"id={applicationUser.OrganizationId}", "999960bff18111e799160126c7e9f568", applicationUser.Id);

                if (org != null)
                {
                    model.Address = org.Address;
                }

                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "63e960bff18111e79916012cc8e9f005", applicationUser.Id);
            }

            return new JsonResult(new { Code = "200", Message = "保存成功。" });
        }

        [Authorize(Roles = "GoodsOwner")]
        [HttpPost]
        public async Task<IActionResult> GoodsShelve(GoodsIdModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = "http://goodsservice.gooios.com/api/goods/v1/shelvegoods";

            var jsonObj = JsonConvert.SerializeObject(model);
            await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "63e960bff18111e79916012cc8e9f005", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "上架成功。" });
        }

        [Authorize(Roles = "GoodsOwner")]
        [HttpPost]
        public async Task<IActionResult> GoodsUnShelve(GoodsIdModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = "http://goodsservice.gooios.com/api/goods/v1/unshelvegoods";

            var jsonObj = JsonConvert.SerializeObject(model);
            await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "63e960bff18111e79916012cc8e9f005", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "下架成功。" });
        }

        [Authorize(Roles = "ServiceOwner")]
        [HttpGet]
        public async Task<IActionResult> ServiceDetail(string id = null)
        {
            ViewBag.Operation = string.IsNullOrEmpty(id?.Trim()) ? "add" : "update";
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var categories = await HttpRequestHelper.GetAsync<List<CategoryModel>>("http://fancyservice.gooios.com/api/category/v1/getbymark", "mark=Service", "768960bff18111e79916016898e9f885", applicationUser.Id);

            var serviceCategories = categories.Where(o => string.IsNullOrEmpty(o.ParentId)).Select(item =>
            {
                return new CategoryModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Children = categories.Where(g => g.ParentId == item.Id).ToList()
                };
            }).ToList();
            ViewBag.Categories = serviceCategories;
            ViewBag.CategoriesJson = JsonConvert.SerializeObject(serviceCategories);

            ServiceModel model = string.IsNullOrEmpty(id) ? null : await HttpRequestHelper.GetAsync<ServiceModel>($"http://fancyservice.gooios.com/api/service/v1/getbyid", $"id={id}", "768960bff18111e79916016898e9f885", applicationUser.Id);

            return View(model);
        }

        [Authorize(Roles = "ServiceOwner")]
        [HttpPost]
        public async Task<IActionResult> ServiceDetail(ServiceModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            model.Category = string.IsNullOrEmpty(model.Category) ? "" : model.Category;
            model.SubCategory = (string.IsNullOrEmpty(model.SubCategory) || model.SubCategory == "请选择") ? null : model.SubCategory;

            var url = "http://fancyservice.gooios.com/api/service/v1";
            var ourl = "http://organizationservice.gooios.com/api/organization/v1/getbyid";
            if (string.IsNullOrEmpty(model.Id))
            {
                model.OrganizationId = applicationUser.OrganizationId;

                var org = await HttpRequestHelper.GetAsync<OrganizationDTO>(ourl, $"id={applicationUser.OrganizationId}", "999960bff18111e799160126c7e9f568", applicationUser.Id);

                if (org != null)
                {
                    model.Station = org.Address;
                }

                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PostNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            }
            else
            {
                model.OrganizationId = applicationUser.OrganizationId;

                var org = await HttpRequestHelper.GetAsync<OrganizationDTO>(ourl, $"id={applicationUser.OrganizationId}", "999960bff18111e799160126c7e9f568", applicationUser.Id);

                if (org?.Address!=null)
                {
                    model.Station = org.Address;
                }

                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            }

            return new JsonResult(new { Code = "200", Message = "保存成功。" });
        }

        [Authorize(Roles = "ServiceOwner")]
        [HttpPost]
        public async Task<IActionResult> ServiceDelete(ServiceIdModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = $"http://fancyservice.gooios.com/api/service/v1/delete?id={model.Id}";

            var jsonObj = JsonConvert.SerializeObject(model);
            await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "删除成功。" });
        }

        [Authorize(Roles = "ServicerOwner")]
        [HttpGet]
        public async Task<IActionResult> ServicerDetail(string id = null)
        {
            ViewBag.Operation = string.IsNullOrEmpty(id?.Trim()) ? "add" : "update";
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var categories = await HttpRequestHelper.GetAsync<List<CategoryModel>>("http://fancyservice.gooios.com/api/category/v1/getbymark", "mark=Servicer", "768960bff18111e79916016898e9f885", applicationUser.Id);

            var serviceCategories = categories.Where(o => string.IsNullOrEmpty(o.ParentId)).Select(item =>
            {
                return new CategoryModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    Children = categories.Where(g => g.ParentId == item.Id).ToList()
                };
            }).ToList();
            ViewBag.Categories = serviceCategories;
            ViewBag.CategoriesJson = JsonConvert.SerializeObject(serviceCategories);

            ServicerModel model = string.IsNullOrEmpty(id) ? null : await HttpRequestHelper.GetAsync<ServicerModel>($"http://fancyservice.gooios.com/api/servicer/v1/getbyid", $"id={id}", "768960bff18111e79916016898e9f885", applicationUser.Id);

            return View(model);
        }

        [Authorize(Roles = "ServicerOwner")]
        [HttpPost]
        public async Task<IActionResult> ServicerDetail(ServicerModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            model.Category = string.IsNullOrEmpty(model.Category) ? "" : model.Category;
            model.SubCategory = (string.IsNullOrEmpty(model.SubCategory)||model.SubCategory=="请选择") ? null : model.SubCategory;
            model.PersonalIntroduction = string.IsNullOrEmpty(model.PersonalIntroduction) ? "" : model.PersonalIntroduction;
            var url = "http://fancyservice.gooios.com/api/servicer/v1";
            if (string.IsNullOrEmpty(model.Id))
            {
                model.OrganizationId = applicationUser.OrganizationId;
                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PostNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            }
            else
            {
                model.OrganizationId = applicationUser.OrganizationId;
                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            }

            return new JsonResult(new { Code = "200", Message = "保存成功。" });
        }

        [Authorize(Roles = "ServicerOwner")]
        [HttpPost]
        public async Task<IActionResult> ServicerDelete(ServicerIdModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = $"http://fancyservice.gooios.com/api/servicer/v1/suspend?id={model.Id}";

            var jsonObj = JsonConvert.SerializeObject(model);
            await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "768960bff18111e79916016898e9f885", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "删除成功。" });
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpGet]
        public async Task<IActionResult> OrderDetail(string id = null)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");


            OrderModel model = string.IsNullOrEmpty(id) ? null : await HttpRequestHelper.GetAsync<OrderModel>($"http://orderservice.gooios.com/api/order/v1/orderid/{id}", "", "77e960be918111e709189226c7e9f002", applicationUser.Id);


            return View(model);
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpPost]
        public async Task<IActionResult> ModifyPrice(ModifyOrderModel model)
        {
            var orderId = model.OrderId;
            var amount = model.Amount;

            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = $"http://orderservice.gooios.com/api/order/v1/orderid/{orderId}";

            var order = await HttpRequestHelper.GetAsync<OrderModel>(url, "", "77e960be918111e709189226c7e9f002", applicationUser.Id);

            if (order != null)
            {
                order.PayAmount = amount;
                var jsonObj = JsonConvert.SerializeObject(order);
                await HttpRequestHelper.PutNoResultAsync("http://orderservice.gooios.com/api/order/v1/modify", jsonObj, "77e960be918111e709189226c7e9f002", applicationUser.Id);
                return new JsonResult(new { Code = "200", Message = "提交成功。" });
            }
            else
            {
                return new JsonResult(new { Code = "400", Message = "提交失败，未找到订单。" });
            }
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpPost]
        public async Task<IActionResult> ModifyRemark(ModifyOrderModel model)
        {
            var orderId = model.OrderId;
            var remark = model.Remark;
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            var url = $"http://orderservice.gooios.com/api/order/v1/orderid/{orderId}";

            var order = await HttpRequestHelper.GetAsync<OrderModel>(url, "", "77e960be918111e709189226c7e9f002", applicationUser.Id);

            if (order != null)
            {
                order.Remark = remark;
                var jsonObj = JsonConvert.SerializeObject(order);
                await HttpRequestHelper.PutNoResultAsync("http://orderservice.gooios.com/api/order/v1/modify", jsonObj, "77e960be918111e709189226c7e9f002", applicationUser.Id);
                return new JsonResult(new { Code = "200", Message = "保存成功。" });
            }
            else
            {
                return new JsonResult(new { Code = "400", Message = "保存失败，未找到订单。" });
            }
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpPost]
        public async Task<IActionResult> OrderShip(string orderId)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });
            
            await HttpRequestHelper.PutNoResultAsync($"http://orderservice.gooios.com/api/order/v1/shipped?orderId={orderId}", "", "77e960be918111e709189226c7e9f002", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "发货成功。" });
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpPost]
        public async Task<IActionResult> OrderCancel(string orderId)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });

            await HttpRequestHelper.PutNoResultAsync($"http://orderservice.gooios.com/api/order/v1/cancel?orderId={orderId}", "", "77e960be918111e709189226c7e9f002", applicationUser.Id);
            return new JsonResult(new { Code = "200", Message = "取消成功。" });
        }

        [Authorize(Roles = "OrderOwner,ServiceOwner,ServicerOwner")]
        [HttpGet]
        public async Task<IActionResult> ReservationDetail(string id = null)
        {
            ViewBag.Operation = string.IsNullOrEmpty(id?.Trim()) ? "add" : "update";
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            return View();
        }

        #region Topic detail

        [Authorize(Roles = "TopicOwner")]
        [HttpGet]
        public async Task<IActionResult> TopicDetail(string id = null)
        {
            ViewBag.Operation = string.IsNullOrEmpty(id?.Trim()) ? "add" : "update";
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");
            
            TopicModel model = string.IsNullOrEmpty(id) ? null : await HttpRequestHelper.GetAsync<TopicModel>($"http://activityservice.gooios.com/api/topic/v1/byid", $"topicId={id}", "83e960bff18221e39916012cc8e9f609", applicationUser.Id);

            return View(model);
        }

        [Authorize(Roles = "TopicOwner")]
        [HttpPost]
        public async Task<IActionResult> TopicDetail(TopicModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return new JsonResult(new { Code = "400", Message = "用户登录异常，请先登录。" });

            model.Introduction = string.IsNullOrEmpty(model.Introduction) ? "" : model.Introduction;
            var url = "http://activityservice.gooios.com/api/topic/v1";
            var ourl = "http://organizationservice.gooios.com/api/organization/v1/getbyid";
            var imgServiceUrl = "https://imageservice.gooios.com/api/image/v1/";

            var org = await HttpRequestHelper.GetAsync<OrganizationDTO>(ourl, $"id={applicationUser.OrganizationId}", "999960bff18111e799160126c7e9f568", applicationUser.Id);
            if (org != null)
            {
                if (model.Address == null || string.IsNullOrEmpty(model.Address.StreetAddress))
                {
                    model.Address = org.Address;
                }
                model.OrganizationName = org.ShortName;
                model.CreatorName = org.ShortName;

                imgServiceUrl += org.LogoImageId;
                var logoimg= await HttpRequestHelper.GetAsync<ImageDTO>(imgServiceUrl, "", "63e960bff18111e799160126c7e9f004", applicationUser.Id);

                model.CreatorPortraitUrl = logoimg?.HttpPath;
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                model.OrganizationId = applicationUser.OrganizationId;
                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PostNoResultAsync(url, jsonObj, "83e960bff18221e39916012cc8e9f609", applicationUser.Id);
            }
            else
            {
                model.OrganizationId = applicationUser.OrganizationId;
                var jsonObj = JsonConvert.SerializeObject(model);
                await HttpRequestHelper.PutNoResultAsync(url, jsonObj, "83e960bff18221e39916012cc8e9f609", applicationUser.Id);
            }

            return new JsonResult(new { Code = "200", Message = "保存成功。" });
        }

        #endregion

        [HttpGet]
        public IActionResult Test()
        {
            return View();
        }
    }

    public class GoodsIdModel
    {
        public string Id { get; set; }
    }

    public class ServiceIdModel
    {
        public string Id { get; set; }
    }

    public class ServicerIdModel
    {
        public string Id { get; set; }
    }

    public class ModifyOrderModel
    {
        public string OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Remark { get; set; }
    }
    public class ImageDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string HttpPath { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ImageBase64Content { get; set; } = string.Empty;
    }
}