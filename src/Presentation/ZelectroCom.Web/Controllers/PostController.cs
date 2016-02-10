using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels;
using ZelectroCom.Web.ViewModels.Home;
using ZelectroCom.Web.ViewModels.Post;
using ZelectroCom.Web.ViewModels.Section;

namespace ZelectroCom.Web.Controllers
{
    public class PostController : BaseWebController
    {
        private const int PageSize = 8;
        private readonly IArticleService _articleService;
        public PostController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Preview(int id)
        {
            Article model = _articleService.GetById(id);

            if (model == null)
            {
                throw new ArgumentException(string.Format("Article with id = {0} was not found", id));
            }

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

            if (model == null)
            {
                throw new ArgumentException(string.Format("Article with id = {0} was not found", id));
            }

            ArticleVm vm = Mapper.Map<Article, ArticleVm>(model);

            model.ViewsCount++;
            _articleService.Update(model);

            return View(vm);
        }

        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "ArticlesUpdate", VaryByParam = "page")]
        [ChildActionOnly]
        public ActionResult NewPosts(int page = 0)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var articles = _articleService.GetArticlesForPage(PageSize, page, out isLastPage, out isFirstPage, x => x.PublishTime);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { UrlNext = Url.Action("Index", "Home", new { page = page + 1}),
                UrlBack = Url.Action("Index", "Home", new { page = page - 1 }),
                PostsList = posts, IsLastPage = isLastPage, IsFirstPage = isFirstPage };

            return PartialView("_PostsList", vm);
        }

        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "ArticlesUpdate", VaryByParam = "page")]
        [ChildActionOnly]
        public ActionResult BestPosts(int page = 0)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var articles = _articleService.GetArticlesForPage(PageSize, page, out isLastPage, out isFirstPage, x => x.Rating);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { UrlNext = Url.Action("Best", "Home", new { page = page + 1 }),
                UrlBack = Url.Action("Best", "Home", new { page = page - 1 }),
                PostsList = posts, IsLastPage = isLastPage, IsFirstPage = isFirstPage };

            return PartialView("_PostsList", vm);
        }

        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "ArticlesUpdate", VaryByParam = "page")]
        [ChildActionOnly]
        public ActionResult PopularPosts(int page = 0)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var articles = _articleService.GetArticlesForPage(PageSize, page, out isLastPage, out isFirstPage, x => x.ViewsCount);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { UrlNext = Url.Action("Popular", "Home", new { page = page + 1 }),
                UrlBack = Url.Action("Popular", "Home", new { page = page - 1 }),
                PostsList = posts, IsLastPage = isLastPage, IsFirstPage = isFirstPage };

            return PartialView("_PostsList", vm);
        }

        //TODO: remove (temporary for output cache)
        [OutputCache(Duration = 3600, VaryByCustom = "SectionsArticlesUpdate", VaryByParam = "id;path;page")]
        [ChildActionOnly]
        public ActionResult SectionPosts(int id, string path, int page = 0)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var articles = _articleService.GetArticlesForPage(PageSize, page, out isLastPage, out isFirstPage,
                x => x.PublishTime, isAscOrder: false, sectionId: id);
            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm()
            {
                UrlNext = string.Format("/{0}/{1}", path, page + 1),
                UrlBack = string.Format("/{0}/{1}", path, page - 1),
                PostsList = posts, IsLastPage = isLastPage, IsFirstPage = isFirstPage };

            return PartialView("_PostsList", vm);
        }

    }
}