using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZelectroCom.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult NoPageFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}