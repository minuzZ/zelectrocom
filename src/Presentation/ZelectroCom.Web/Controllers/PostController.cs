using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels;
using ZelectroCom.Web.ViewModels.Home;
using ZelectroCom.Web.ViewModels.Post;

namespace ZelectroCom.Web.Controllers
{
    public class PostController : Controller
    {
        private const int pageSize = 8;
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

            return View(vm);
        }

        public ActionResult Article(int id)
        {
            Article model = _articleService.GetById(id);

            ArticleVm vm = Mapper.Map<Article, ArticleVm>(model);

            return View(vm);
        }

        public ActionResult NewPosts(int page = 0)
        {
            var articles = _articleService.GetArticlesForPage(pageSize, page, x => x.PublishTime);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { ScrollUrl = Url.Action("NewPosts", new { page = page + 1}), PostsList = posts };

            return PartialView("_PostsList", vm);
        }

        public ActionResult BestPosts(int page = 0)
        {
            var articles = _articleService.GetArticlesForPage(pageSize, page, x => x.Rating);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { ScrollUrl = Url.Action("BestPosts", new { page = page + 1 }), PostsList = posts };

            return PartialView("_PostsList", vm);
        }

        public ActionResult PopularPosts(int page = 0)
        {
            var articles = _articleService.GetArticlesForPage(pageSize, page, x => x.ViewsCount);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { ScrollUrl = Url.Action("PopularPosts", new { page = page + 1 }), PostsList = posts };

            return PartialView("_PostsList", vm);
        }

        public ActionResult SectionPosts(int id, int page = 0)
        {
            var articles = _articleService.GetArticlesForPage(pageSize, page, x => x.PublishTime, isAscOrder: false, sectionId:id);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { ScrollUrl = Url.Action("SectionPosts", new { id, page = page + 1 }), PostsList = posts };

            return PartialView("_PostsList", vm);
        }

    }
}