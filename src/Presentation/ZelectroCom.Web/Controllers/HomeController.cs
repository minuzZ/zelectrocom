using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ZelectroCom.Data.Mapping;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels.Search;
using ZelectroCom.Web.ViewModels.Section;

namespace ZelectroCom.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISectionService _sectionService;
        public HomeController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [ChildActionOnly]
        public ActionResult HomeSections()
        {
            var sectionsList = _sectionService
                .GetActiveSections()
                .Where(x => !x.IsHidden)
                .OrderBy(x => x.Order)
                .Select(x => Mapper.Map(x, new SectionVm()));

            return PartialView("_HomeSectionsPartial", sectionsList);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Best()
        {
            return View();
        }

        public ActionResult Popular()
        {
            return View();
        }

        public ActionResult Search(SearchVm searchVm)
        {
            return View(searchVm);
        }

        public ActionResult Section(int id)
        {
            var model = _sectionService.GetById(id);
            var vm = Mapper.Map<Section, SectionVm>(model);
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}