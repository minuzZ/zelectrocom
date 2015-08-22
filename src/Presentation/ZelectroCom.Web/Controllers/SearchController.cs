using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZelectroCom.Web.Controllers
{
    public class SearchController : Controller
    {
        [ChildActionOnly]
        public ActionResult SearchWidget()
        {
            return PartialView("_SearchPartial");
        }
	}
}