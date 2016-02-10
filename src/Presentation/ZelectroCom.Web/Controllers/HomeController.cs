using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using ZelectroCom.Data.Mapping;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels.Search;
using ZelectroCom.Web.ViewModels.Section;

namespace ZelectroCom.Web.Controllers
{
    public class HomeController : BaseWebController
    {
        private readonly ISectionService _sectionService;

        public HomeController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [ChildActionOnly]
        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "SectionsUpdate")]
        public ActionResult HomeSections()
        {
            var sectionsList = GetSections();
            return PartialView("_HomeSectionsPartial", sectionsList);
        }

        public ActionResult Index(int page = 0)
        {
            return View(page);
        }

        public ActionResult Best(int page = 0)
        {
            return View(page);
        }

        public ActionResult Popular(int page = 0)
        {
            return View(page);
        }

        public ActionResult Search(SearchVm searchVm)
        {
            return View(searchVm);
        }

        public ActionResult SearchPage()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "SectionsUpdate", Location = OutputCacheLocation.Server)]
        public ActionResult SectionsPage()
        {
            var sectionsList = GetSections();
            return View(sectionsList);
        }

        public ActionResult Section(int id, int page = 0)
        {
            var model = _sectionService.GetById(id);

            if (model == null)
                throw new ArgumentException(string.Format("Section with id = {0} was not found", id));

            var vm = Mapper.Map<Section, SectionVm>(model);
            vm.Page = page;
            return View(vm);
        }

        public ActionResult Contact()
        {
            return View();
        }

        private IEnumerable<SectionVm> GetSections()
        {
            //TODO: cache
             return _sectionService
                .GetActiveSections()
                .Where(x => !x.IsHidden)
                .OrderBy(x => x.Order)
                .Select(x => Mapper.Map(x, new SectionVm()));
        }
    }
}