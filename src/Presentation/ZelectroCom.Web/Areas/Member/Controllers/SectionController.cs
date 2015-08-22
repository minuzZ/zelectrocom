using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;
using ZelectroCom.Web.Infrastructure.Attributes;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Admin")]
    [NoCache]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSections()
        {
            var records = Mapper.Map<IEnumerable<Section>, IEnumerable<SectionVM>>(_sectionService.GetAll());
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(SectionVM section)
        {
            _sectionService.CreateOrUpdate(Mapper.Map<SectionVM, Section>(section));
            return Json(true);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            _sectionService.Delete(id);
            return Json(true);
        }
	}
}