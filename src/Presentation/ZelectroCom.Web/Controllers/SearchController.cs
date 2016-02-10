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
    public class SearchController : BaseWebController
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

        [ChildActionOnly]
        public ActionResult Search(SearchVm searchVm)
        {
            if (searchVm.Page < 0)
            {
                throw new ArgumentException("Page less than 0");
            }

            bool isLastPage, isFirstPage;
            var articles = _articleService.GetArticlesForPage(pageSize, searchVm.Page, out isLastPage, 
                out isFirstPage, x => x.Rating, _articleService.SearchArticles(searchVm.SearchText));

            if (articles == null)
                return new EmptyResult();

            var posts = Mapper.Map<IEnumerable<Article>, IEnumerable<PostIndexVm>>(articles);

            SearchVm vmBack = new SearchVm() { Page = searchVm.Page - 1, SearchText = searchVm.SearchText};
            searchVm.Page += 1;

            var vm = new PostsListVm()
            {
                UrlNext = Url.Action("Search", "Home", searchVm),
                UrlBack = Url.Action("Search", "Home", vmBack),
                PostsList = posts, IsLastPage = isLastPage, IsFirstPage = isFirstPage };

            return PartialView("../Post/_PostsList", vm);
        }
	}
}