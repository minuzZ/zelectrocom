using System.Security;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.ViewModels;

namespace ZelectroCom.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IArticleService _articleService;
        public PostController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Preview(int id)
        {
            Article model = _articleService.GetById(id);

            if (model.AuthorId != User.Identity.GetUserId())
            {
                throw new SecurityException("User is not authorized to open this record");
            }

            PreviewArticleVm vm = Mapper.Map<Article, PreviewArticleVm>(model);
            vm.Author = User.Identity.Name;

            return View(vm);
        }
    }
}