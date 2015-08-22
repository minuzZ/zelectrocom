using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProfileSettings()
        {
            return View();
        }

        public ActionResult ProfileInfo()
        {
            return View();
        }
    }
}
