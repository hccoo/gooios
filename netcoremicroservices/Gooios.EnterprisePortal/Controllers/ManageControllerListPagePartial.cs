using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gooios.EnterprisePortal.Utilities;
using Gooios.EnterprisePortal.Models.ManageViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Gooios.EnterprisePortal.Controllers
{
    public partial class ManageController
    {
        [Authorize(Roles = "GoodsOwner")]
        [HttpGet]
        public async Task<IActionResult> GoodsManagement(string key = "", string category = "", string subCategory = "", int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://goodsservice.gooios.com/api/goods/v1/getpaginggoods";
            var parameters = $"key={key}&category={category}&subcategory={subCategory}&pageindex={pageIndex}&pagesize=20";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<GoodsModel>>(apiUrl, parameters, "63e960bff18111e79916012cc8e9f005");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        [Authorize(Roles = "ServiceOwner")]
        [HttpGet]
        public async Task<IActionResult> ServiceManagement(int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://fancyservice.gooios.com/api/service/v1/getbyorganizationid";
            var parameters = $"organizationid={applicationUser.OrganizationId}&pageindex={pageIndex}";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<ServiceModel>>(apiUrl, parameters, "768960bff18111e79916016898e9f885");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        [Authorize(Roles = "ServicerOwner")]
        [HttpGet]
        public async Task<IActionResult> ServicerManagement(int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://fancyservice.gooios.com/api/servicer/v1/getbyorganizationid";
            var parameters = $"organizationid={applicationUser.OrganizationId}&pageindex={pageIndex}";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<ServicerModel>>(apiUrl, parameters, "768960bff18111e79916016898e9f885");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        [Authorize(Roles = "OrderOwner,ServiceOwner,ServicerOwner")]
        [HttpGet]
        public async Task<IActionResult> ReservationManagement(int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://fancyservice.gooios.com/api/reservation/v1/getbyorganizationid";
            var parameters = $"organizationid={applicationUser.OrganizationId}&pageindex={pageIndex}";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<ReservationModel>>(apiUrl, parameters, "768960bff18111e79916016898e9f885");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        [Authorize(Roles = "TopicOwner")]
        [HttpGet]
        public async Task<IActionResult> TopicManagement(int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://activityservice.gooios.com/api/topic/v1/byorganizationid";
            var parameters = $"organizationid={applicationUser.OrganizationId}&pageindex={pageIndex}&pagesize=10";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<TopicModel>>(apiUrl, parameters, "83e960bff18221e39916012cc8e9f609");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        [Authorize(Roles = "OrderOwner")]
        [HttpGet]
        public async Task<IActionResult> OrderManagement(int pageIndex = 1)
        {
            var applicationUser = await _userManager.GetUserAsync(new System.Security.Claims.ClaimsPrincipal(User.Identity));
            if (applicationUser == null || string.IsNullOrEmpty(applicationUser.OrganizationId)) return RedirectToAction("Login", "Account");

            var apiUrl = "http://orderservice.gooios.com/api/order/v1/byorganizationid";
            var parameters = $"organizationid={applicationUser.OrganizationId}&pageindex={pageIndex}";
            var viewModel = await HttpRequestHelper.GetAsync<IEnumerable<OrderModel>>(apiUrl, parameters, "77e960be918111e709189226c7e9f002");

            if (viewModel == null || viewModel.Count() == 0)
            {
                if (pageIndex > 1) pageIndex--;
            }

            ViewBag.CurrentPageIndex = pageIndex;

            return View(viewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> ServicerCommentsManagement()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> ServiceCommentsManagement()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GoodsCommentsManagement()
        //{
        //    return View();
        //}
    }
}