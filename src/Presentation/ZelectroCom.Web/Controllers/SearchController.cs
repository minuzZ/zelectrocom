using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.ViewModels.Home;
using ZelectroCom.Web.ViewModels.Search;

namespace ZelectroCom.Web.Controllers
{
    public class SearchController : Controller
    {
        private const int pageSize = 8;
        private readonly IArticleService _articleService;

        public SearchController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [ChildActionOnly]
        public ActionResult SearchWidget()
        {
            return PartialView("_SearchPartial");
        }

        public ActionResult Search(SearchVm searchVm, int page = 0)
        {
            var articles = _articleService.GetArticlesForPage(pageSize, page, x => x.Rating, _articleService.SearchArticles(searchVm.SearchText));

            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            var vm = new PostsListVm() { ScrollUrl = Url.Action("Search", new { page = page + 1 }), PostsList = posts };

            return PartialView("../Post/_PostsList", vm);
        }
	}
}