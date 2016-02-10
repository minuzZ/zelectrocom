using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels.Home;
using ZelectroCom.Web.ViewModels.ZDev;

namespace ZelectroCom.Web.Controllers
{
    public class ZDevController : Controller
    {
        private const int PageSize = 8;
        private readonly IZDevService _zDevService;

        public ZDevController(IZDevService zDevService)
        {
            _zDevService = zDevService;
        }

        public ActionResult Index(int page = 0)
        {
            return View(page);
        }

        [ChildActionOnly]
        public ActionResult ZProducts(int page = 0)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var zProds = _zDevService.GetProductsForPage(PageSize, page, out isLastPage, out isFirstPage);
            if (zProds == null)
                return new EmptyResult();

            var products = Mapper.Map<IEnumerable<ZDev>, IEnumerable<ZDevIndexVm>>(zProds);

            var vm = new ZItemsListVm()
            {
                UrlNext = Url.Action("Index", "ZDev", new { page = page + 1 }),
                UrlBack = Url.Action("Index", "ZDev", new { page = page - 1 }),
                ProductsList = products,
                IsLastPage = isLastPage,
                IsFirstPage = isFirstPage
            };

            return PartialView("_ZProducts", vm);
        }
    }
}