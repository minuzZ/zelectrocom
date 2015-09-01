using System.Web.Mvc;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Controllers
{
    public class PostController : Controller
    {
        [HeaderAntiForgery]
        [HttpPost]
        public ActionResult Preview(DraftVm memberDraftVm)
        {
            return View();
        }
    }
}