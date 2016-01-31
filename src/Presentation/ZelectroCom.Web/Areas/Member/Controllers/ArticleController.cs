using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Web.Mvc;
using AutoMapper;
using BForms.Grid;
using BForms.Models;
using BForms.Mvc;
using Microsoft.AspNet.Identity;
using RequireJsNet;
using WebGrease.Css.Extensions;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Infrastructure.Filters;
using ZelectroCom.Web.Infrastructure.Helpers;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ArticleController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly IArticleService _articleService;
        private readonly ICustomUrlService _customUrlService;

        private readonly IHtmlParserHelper _htmlParserHelper;

        private readonly FakeArticleDraftRepository _fakeArticleDraftRepository;
        private readonly FakeArticlePubRepository _fakeArticlePubRepository;
        private readonly FakeArticlePubReqRepository _fakeArticlePubReqRepository;
        public ArticleController(ISectionService sectionService, IArticleService articleService, IHtmlParserHelper htmlParserHelper, ICustomUrlService customUrlService)
        {
            _sectionService = sectionService;
            _articleService = articleService;
            _customUrlService = customUrlService;
            _htmlParserHelper = htmlParserHelper;

            _fakeArticleDraftRepository = new FakeArticleDraftRepository(articleService);
            _fakeArticlePubRepository = new FakeArticlePubRepository(articleService);
            _fakeArticlePubReqRepository = new FakeArticlePubReqRepository(articleService);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _fakeArticleDraftRepository.User = _fakeArticlePubRepository.User = User;
            }
        }

        public ActionResult DraftsList()
        {
            var gridModel = _fakeArticleDraftRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() {Page = 1, PageSize = 10});

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

        public ActionResult Publications()
        {
            var gridModel = _fakeArticlePubRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 10 });

            var model = new PublicationsVm()
            {
                Grid = gridModel,
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl",  Url.Action("PublicationPager")},
                {"editUrl", Url.Action("Draft")}
            };

            RequireJsOptions.Add("index", options);

            return View(model);
        }

        public ActionResult PubRequests()
        {
            var gridModel = _fakeArticlePubReqRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 10 });

            var model = new PubRequestsVm()
            {
                Grid = gridModel,
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl",  Url.Action("PubReqPager")},
                {"editUrl", Url.Action("Draft")}
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
                var viewModel = _fakeArticleDraftRepository.ToBsGridViewModel(settings, out count).Wrap<DraftListVm>(x => x.Grid);

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

        [NoAntiForgeryCheck]
        public BsJsonResult PublicationPager(BsGridBaseRepositorySettings settings)
        {
            var msg = string.Empty;
            var status = BsResponseStatus.Success;
            var html = string.Empty;
            var count = 0;

            try
            {
                var viewModel = _fakeArticlePubRepository.ToBsGridViewModel(settings, out count).Wrap<PublicationsVm>(x => x.Grid);

                html = this.BsRenderPartialView("_PublicationsGrid", viewModel);
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

        [NoAntiForgeryCheck]
        public BsJsonResult PubReqPager(BsGridBaseRepositorySettings settings)
        {
            var msg = string.Empty;
            var status = BsResponseStatus.Success;
            var html = string.Empty;
            var count = 0;

            try
            {
                var viewModel = _fakeArticlePubReqRepository.ToBsGridViewModel(settings, out count).Wrap<PubRequestsVm>(x => x.Grid);
                html = this.BsRenderPartialView("_PubRequestsGrid", viewModel);
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

                    if (authorId == article.AuthorId || User.IsInRole("Admin"))
                    {
                        article.ArticleState = ArticleState.Deleted;
                        _articleService.Update(article);
                    }
                    else
                    {
                        throw new SecurityException("User is not authorized to delete this record");
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
                {"postUrl",  Url.Action("PostDraft")},
                {"successUrl",  Url.Action("PostSuccess")},
                {"returnUrl",  Url.Action("ReturnDraft")},
                {"submitUrl",  Url.Action("PublishDraft")},
                {"changesUrl",  Url.Action("ProposeChanges")}
            };

            RequireJsOptions.Add("index", options);

            if (_articleService.IsDraft(model) || model.ArticleState == ArticleState.New)
            {
                ViewBag.SuccessText = "Черновик был успешно сохранен";
            }
            else if (_articleService.IsPublished(model))
            {
                ViewBag.SuccessText = "Публикация успешно изменена";
            }
            else if (_articleService.IsPosted(model))
            {
                ViewBag.SuccessText = "Успешно!";
            }

            return View(vm);
        }

        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult SaveDraft(DraftVm draft)
        {
            string errorText = null;
            string indexHtml = null;
            Article model = null;
            CustomUrl customUrl = null;
            if (ModelState.IsValid)
            {
                var article = _articleService.GetById(draft.Id);
                model = Mapper.Map(draft, article);


                customUrl = CreateOrUpdateCustomUrl(draft);

                if (model.ArticleState == ArticleState.New)
                {
                    model.ArticleState = ArticleState.Draft;
                }

                if (model.ArticleState == ArticleState.Article)
                {
                    if (!_htmlParserHelper.GetIndexHtmlFromArticleText(model.Text, out indexHtml))
                    {
                        errorText = "Статья не содержит линии разрыва печати. Без неё публиковать нельзя!";
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                errorText = "Поля заполнены не верно. Черновик не был сохранен.";
            }

            if (errorText != null)
            {
                ModelState.AddFormError("Ошибка: ", errorText);

                return new BsJsonResult(
                    new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                    BsResponseStatus.ValidationError);
            }

            model.IndexHtml = indexHtml;
            _articleService.Update(model);
            if (customUrl != null)
            {
                _customUrlService.CreateOrUpdate(customUrl);
            }

            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }

        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult PostDraft(DraftVm draft)
        {
            Article model = null;
            CustomUrl customUrl = null;

            if (ModelState.IsValid)
            {
                var article = _articleService.GetById(draft.Id);
                model = Mapper.Map(draft, article);
                model.ArticleState = ArticleState.Posted;

                customUrl = CreateOrUpdateCustomUrl(draft);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddFormError("Ошибка: ",
                "Поля заполнены не верно. Черновик не был сохранен и передан на публикацию.");

                return new BsJsonResult(
                    new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                    BsResponseStatus.ValidationError);
            }

            _articleService.Update(model);
            if (customUrl != null)
            {
                _customUrlService.CreateOrUpdate(customUrl);
            }

            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }


        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult ReturnDraft(DraftVm draft)
        {
            Article model = null;
            CustomUrl customUrl = null;
            if (ModelState.IsValid)
            {
                var article = _articleService.GetById(draft.Id);
                model = Mapper.Map(draft, article);
                model.ArticleState = ArticleState.ReturnedDraft;
                customUrl = CreateOrUpdateCustomUrl(draft);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddFormError("Ошибка: ",
                "Поля заполнены не верно. Черновик не был сохранен и не может быть возвращен.");

                return new BsJsonResult(
                    new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                    BsResponseStatus.ValidationError);
            }

            _articleService.Update(model);
            if (customUrl != null)
            {
                _customUrlService.CreateOrUpdate(customUrl);
            }

            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }

        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult PublishDraft(DraftVm draft)
        {
            string errorText = null;
            string indexHtml = null;
            Article model = null;
            CustomUrl customUrl = null;

            if (ModelState.IsValid)
            {
                var article = _articleService.GetById(draft.Id);
                model = Mapper.Map(draft, article);
                model.ArticleState = ArticleState.Article;

                if (string.IsNullOrEmpty(model.Text))
                {
                    errorText = "Отстутствует текст статьи!";
                }
                else if (!_htmlParserHelper.GetIndexHtmlFromArticleText(model.Text, out indexHtml))
                {
                    errorText = "Статья не содержит линии разрыва печати. Без неё публиковать нельзя!";
                }

                customUrl = CreateOrUpdateCustomUrl(draft);
            }

            if (!ModelState.IsValid)
            {
                errorText = "Поля заполнены не верно. Черновик не был сохранен и не может быть опубликован.";
            }

            if (errorText != null)
            {
                ModelState.AddFormError("Ошибка: ", errorText);
                return new BsJsonResult(
                    new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                    BsResponseStatus.ValidationError);
            }

            model.IndexHtml = indexHtml;
            _articleService.Update(model);
            if (customUrl != null)
            {
                _customUrlService.CreateOrUpdate(customUrl);
            }

            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }

        [HttpPost]
        [HeaderAntiForgeryAttribute]
        public BsJsonResult ProposeChanges(DraftVm draft)
        {
            throw new NotImplementedException();
        }

        public ActionResult PostSuccess()
        {
            return View();
        }

        private void InitViewModel(DraftVm vm)
        {
            vm.Sections = new SelectList(_sectionService.GetActiveSections(), "Id", "Name");

            var customUrl = _customUrlService.GetAll().FirstOrDefault(x => x.ContentId == vm.Id && x.ContentType == ContentType.Article);
            if (customUrl != null)
            {
                vm.SeoUrl = customUrl.Url;
            }
        }

        private CustomUrl CreateOrUpdateCustomUrl(DraftVm draft)
        {
            var customUrl = _customUrlService.GetAll().FirstOrDefault(x => x.ContentId == draft.Id && x.ContentType == ContentType.Article);

            if (customUrl == null && !string.IsNullOrEmpty(draft.SeoUrl))
            {
                customUrl = new CustomUrl();
            }

            if (customUrl != null && customUrl.Url != draft.SeoUrl)
            {
                if (!_customUrlService.IsUniquePath(draft.SeoUrl))
                {
                    ModelState.AddModelError("SeoUrl", "Путь не уникален");
                    return null;
                }
                customUrl.Url = draft.SeoUrl;
                customUrl.ContentType = ContentType.Article;
                customUrl.ContentId = draft.Id;
            }

            return customUrl;
        }

        #region BForms

        class FakeArticleDraftRepository : BsBaseGridRepository<Article, DraftRowVm>
        {
            private readonly IArticleService _articleService;

            public IPrincipal User { get; set; }

            internal FakeArticleDraftRepository(IArticleService articleService)
            {
                _articleService = articleService;
            }

            public override IQueryable<Article> Query()
            {
                //get only authors drafts
                var draftsQuery = _articleService.GetDrafts(User.Identity.GetUserId())
                    .AsQueryable();
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

        class FakeArticlePubRepository : BsBaseGridRepository<Article, PublicationRowVm>
        {
            private readonly IArticleService _articleService;
            public IPrincipal User { get; set; }

            internal FakeArticlePubRepository(IArticleService articleService)
            {
                _articleService = articleService;
            }

            public override IQueryable<Article> Query()
            {
                //Get authors publications. All publications for admin
                var draftsQuery = _articleService.GetPublished(User.IsInRole("Admin") ? null : User.Identity.GetUserId()).AsQueryable();
                return draftsQuery;
            }

            public override IOrderedQueryable<Article> OrderQuery(IQueryable<Article> query,
                BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.CreatedDate));
                return ordered;
            }

            public override IEnumerable<PublicationRowVm> MapQuery(IQueryable<Article> query)
            {
                var mapped = query.Select(Mapper.Map<Article, PublicationRowVm>);
                string authorName = query.First().Author.UserName;

                var pubRequestRowVms = mapped as IList<PublicationRowVm> ?? mapped.ToList();
                pubRequestRowVms.ForEach(x => x.AuthorName = authorName);
                return pubRequestRowVms;
            }
        }

        class FakeArticlePubReqRepository : BsBaseGridRepository<Article, PubRequestRowVm>
        {
            private readonly IArticleService _articleService;

            internal FakeArticlePubReqRepository(IArticleService articleService)
            {
                _articleService = articleService;
            }

            public override IQueryable<Article> Query()
            {
                var draftsQuery = _articleService.GetPosted().AsQueryable();
                return draftsQuery;
            }

            public override IOrderedQueryable<Article> OrderQuery(IQueryable<Article> query,
                BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.CreatedDate));
                return ordered;
            }

            public override IEnumerable<PubRequestRowVm> MapQuery(IQueryable<Article> query)
            {
                var mapped = query.Select(Mapper.Map<Article, PubRequestRowVm>);
                string authorName = query.First().Author.UserName;

                var pubRequestRowVms = mapped as IList<PubRequestRowVm> ?? mapped.ToList();
                pubRequestRowVms.ForEach(x => x.AuthorName = authorName);
                return pubRequestRowVms;
            }
        }

        #endregion

    }
}