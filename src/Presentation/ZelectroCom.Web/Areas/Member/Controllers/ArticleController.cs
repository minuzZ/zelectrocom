using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web.Mvc;
using AutoMapper;
using BForms.Grid;
using BForms.Models;
using BForms.Mvc;
using Microsoft.AspNet.Identity;
using RequireJsNet;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Infrastructure;
using ZelectroCom.Web.Infrastructure.Attributes;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ArticleController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly IArticleService _articleService;
        private readonly FakeArticleRepository _fakeArticleRepository;
        public ArticleController(ISectionService sectionService, IArticleService articleService)
        {
            _sectionService = sectionService;
            _articleService = articleService;
            _fakeArticleRepository = new FakeArticleRepository(articleService);
        }

        [NoAntiForgeryCheck]
        public ActionResult DraftsList()
        {
            var gridModel = _fakeArticleRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() {Page = 1, PageSize = 10});

            var model = new DraftListVm()
            {
                Grid = gridModel,
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl",  Url.Action("Pager")},
                {"deleteUrl", Url.Action("Delete")},
                {"draftUrl", Url.Action("Draft")}
            };

            RequireJsOptions.Add("index", options);

            return View(model);
        }

        [NoAntiForgeryCheck]
        public BsJsonResult Pager(BsGridBaseRepositorySettings settings)
        {
            var msg = string.Empty;
            var status = BsResponseStatus.Success;
            var html = string.Empty;
            var count = 0;

            try
            {
                var viewModel = _fakeArticleRepository.ToBsGridViewModel(settings, out count).Wrap<DraftListVm>(x => x.Grid);

                html = this.BsRenderPartialView("_DraftsGrid", viewModel);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                status = BsResponseStatus.ServerError;
            }

            return new BsJsonResult(new
            {
                Count = count,
                Html = html
            }, status, msg);
        }

        [HeaderAntiForgeryAttribute]
        public BsJsonResult Delete(List<BsGridRowData<int>> items)
        {
            var msg = string.Empty;
            var status = BsResponseStatus.Success;

            try
            {
                foreach (var item in items)
                {
                    string authorId = User.Identity.GetUserId();
                    var article = _articleService.GetById(item.Id);

                    if (authorId == article.AuthorId)
                    {
                        article.ArticleState = ArticleState.Deleted;
                        _articleService.Update(article);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "<strong>" + Resources.Resources.Error_Server + "!</strong> ";
                status = BsResponseStatus.ServerError;
            }

            return new BsJsonResult(null, status, msg);
        }

        [HttpPost]
        public ActionResult Draft()
        {
            Article model = _articleService.Create(new Article()
            {
                AuthorId = User.Identity.GetUserId(),
                ArticleState = ArticleState.New,
                PublishTime = DateTime.Now
            });

            return RedirectToAction("Draft", new { id = model.Id });
        }

        [HttpGet]
        public ActionResult Draft(int id)
        {
            Article model = _articleService.GetById((int)id);

            if (model.AuthorId != User.Identity.GetUserId())
            {
                throw new SecurityException("User is not authorized to open this record");
            }


            DraftVm vm = Mapper.Map<Article, DraftVm>(model);
            InitViewModel(vm);

            var options = new Dictionary<string, string>
            {
                {"saveDraftUrl",  Url.Action("SaveDraft")},
                {"previewUrl", Url.Action("Preview", "Post", new {area = string.Empty})}
            };

            RequireJsOptions.Add("index", options);

            return View(vm);
        }

        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult SaveDraft(DraftVm draft)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<DraftVm, Article>(draft);
                model.AuthorId = User.Identity.GetUserId();
                model.ArticleState = ArticleState.Draft;
                _articleService.Update(model);
            }
            else
            {
                ModelState.AddFormError("Ошибка: ",
                "Поля заполнены не верно. Черновик не был сохранен.");

                return new BsJsonResult(
                    new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                    BsResponseStatus.ValidationError);
            }
            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PublishDraft")]
        public ActionResult PublishDraft(DraftVm draft)
        {
            throw new NotImplementedException();
        }

        private void InitViewModel(DraftVm vm)
        {
            vm.Sections = new SelectList(_sectionService.GetAll().Where(x => !x.IsHidden), "Id", "Name");
        }

        #region BForms

        class FakeArticleRepository : BsBaseGridRepository<Article, DraftRowVm>
        {
            private readonly IArticleService _articleService;

            internal FakeArticleRepository(IArticleService articleService)
            {
                _articleService = articleService;
            }

            public override IQueryable<Article> Query()
            {
                var draftsQuery = _articleService.GetDrafts().AsQueryable();
                return draftsQuery;
            }

            public override IOrderedQueryable<Article> OrderQuery(IQueryable<Article> query, BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.CreatedDate));
                return ordered;
            }

            public override IEnumerable<DraftRowVm> MapQuery(IQueryable<Article> query)
            {
                var mapped = query.Select(Mapper.Map<Article, DraftRowVm>);
                return mapped;
            }
        }

        #endregion

    }
}