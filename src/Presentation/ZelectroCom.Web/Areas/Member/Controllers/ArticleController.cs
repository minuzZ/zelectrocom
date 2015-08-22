using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Infrastructure.Attributes;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ArticleController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly IArticleService _articleService;
        public ArticleController(ISectionService sectionService, IArticleService articleService)
        {
            _sectionService = sectionService;
            _articleService = articleService;
        }

        public ActionResult DraftsList()
        {
            return View();
        }

        public JsonResult GetDrafts(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total;
            var records = Mapper.Map<IEnumerable<Article>, IEnumerable<DraftListItemVM>>(_articleService
                    .GetDrafts(page, limit, sortBy, direction, searchString, out total));
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Draft(int? id)
        {
            Article model;
            DraftVM vm;
            if (id != null)
            {
                model = _articleService.GetById((int)id);

                if (model.AuthorId != User.Identity.GetUserId())
                {
                    throw new SecurityException("User is not authorized to open this record");
                }
            }
            else
            {
                model = _articleService.Create(new Article()
                {
                    AuthorId = User.Identity.GetUserId(),
                    ArticleState = ArticleState.New,
                    PublishTime = DateTime.Now
                });
            }

            vm = Mapper.Map<Article, DraftVM>(model);
            InitViewModel(vm);

            return View(vm);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            string authorId = User.Identity.GetUserId();
            var item = _articleService.GetById(id);

            if (authorId == item.AuthorId)
            {
                item.ArticleState = ArticleState.Deleted;
                _articleService.Update(item);
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        [MultipleButton(Name="action", Argument="SaveDraft")]
        public ActionResult SaveDraft(DraftVM draft)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<DraftVM, Article>(draft);
                model.AuthorId = User.Identity.GetUserId();
                model.ArticleState = ArticleState.Draft;
                model = _articleService.Update(model);
                return RedirectToAction("Draft", new {id = model.Id});
            }
            draft.Sections = new SelectList(_sectionService.GetAll().Where(x => !x.IsHidden), "Id", "Name");
            return View("Draft", draft);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PublishDraft")]
        public ActionResult PublishDraft(DraftVM draft)
        {
            throw new NotImplementedException();
        }

        private void InitViewModel(DraftVM vm)
        {
            vm.Sections = new SelectList(_sectionService.GetAll().Where(x => !x.IsHidden), "Id", "Name");
        }
    }
}