using System.Web.Mvc;
using ZelectroCom.Web.Infrastructure;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult ClearCache()
        {
            CustomUrlCache.RefreshCache();
            return View();
        }
    }
}