using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZelectroCom.Service;

namespace ZelectroCom.Web.Controllers
{
    public class SectionController : Controller
    {
        private ISectionService _sectionService;
        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }
        [ChildActionOnly]
        public ActionResult HomeSections()
        {
            var sectionsList = _sectionService
                .GetAll()
                .Where(x => !x.IsHidden)
                .OrderBy(x => x.Order)
                .Select(x => x.Name);

            return PartialView("_HomeSectionsPartial", sectionsList);
        }

	}
}