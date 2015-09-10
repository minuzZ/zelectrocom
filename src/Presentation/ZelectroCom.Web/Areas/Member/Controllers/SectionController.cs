using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BForms.Grid;
using BForms.Models;
using BForms.Mvc;
using Microsoft.AspNet.Identity;
using RequireJsNet;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;
using ZelectroCom.Web.Infrastructure.Attributes;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Admin")]
    [NoCache]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly FakeSectionRepository _fakeSectionRepository;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
            _fakeSectionRepository = new FakeSectionRepository(_sectionService);
        }

        public ActionResult Index()
        {
            var gridModel =
                _fakeSectionRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 10 });

            var model = new SectionListVm()
            {
                Grid = gridModel,
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl", Url.Action("Pager")},
                {"deleteUrl", Url.Action("Delete")},
                {"editUrl", Url.Action("Edit")}
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
                var viewModel = _fakeSectionRepository.ToBsGridViewModel(settings, out count).Wrap<SectionListVm>(x => x.Grid);

                html = this.BsRenderPartialView("_SectionsGrid", viewModel);
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
                    var section = _sectionService.GetById(item.Id);
                    section.SectionState = SectionState.Deleted;
                    _sectionService.Update(section);
                }
            }
            catch (Exception ex)
            {
                msg = "<strong>" + Resources.Resources.Error_Server + "!</strong> ";
                status = BsResponseStatus.ServerError;
            }

            return new BsJsonResult(null, status, msg);
        }

        public ActionResult Add()
        {
            SectionVm vm = new SectionVm();
            PrepareViewModel(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(SectionVm section)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<SectionVm, Section>(section);
                model.SectionState = SectionState.Active;
                _sectionService.Create(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var section = _sectionService.GetById(id);
            SectionVm vm = Mapper.Map<Section, SectionVm>(section);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(SectionVm section)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<SectionVm, Section>(section);
                model.SectionState = SectionState.Active;
                _sectionService.Update(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        private void PrepareViewModel(SectionVm vm)
        {
            vm.Order = new BsRangeItem<int>
            {
                MinValue = 0,
                MaxValue = 100,
                TextValue = "0-100",
                Display = Resources.Resources.SectionVm_Order
            };
        }

        #region BForms

        private class FakeSectionRepository : BsBaseGridRepository<Section, SectionRowVm>
        {
            private readonly ISectionService _sectionService;

            internal FakeSectionRepository(ISectionService sectionService)
            {
                _sectionService = sectionService;
            }

            public override IQueryable<Section> Query()
            {
                var sectionsQuery = _sectionService.GetActiveSections().AsQueryable();
                return sectionsQuery;
            }

            public override IOrderedQueryable<Section> OrderQuery(IQueryable<Section> query,
                BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.Order));
                return ordered;
            }

            public override IEnumerable<SectionRowVm> MapQuery(IQueryable<Section> query)
            {
                var mapped = query.Select(Mapper.Map<Section, SectionRowVm>);
                return mapped;
            }
        }

        #endregion
    }
}