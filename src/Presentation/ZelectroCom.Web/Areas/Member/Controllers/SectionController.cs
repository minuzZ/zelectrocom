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
using ZelectroCom.Web.Infrastructure.Helpers;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Admin")]
    [NoCache]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ICustomUrlService _customUrlService;
        private readonly FakeSectionRepository _fakeSectionRepository;

        public SectionController(ISectionService sectionService, ICustomUrlService customUrlService)
        {
            _sectionService = sectionService;
            _customUrlService = customUrlService;
            _fakeSectionRepository = new FakeSectionRepository(_sectionService, _customUrlService);
        }

        public ActionResult Index()
        {
            var gridModel =
                _fakeSectionRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 10 });

            var model = new SectionListVm()
            {
                Grid = gridModel
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
                    var customUrl = _customUrlService.GetAll().FirstOrDefault(x => x.ContentId == section.Id && x.ContentType == ContentType.Section);
                    if (customUrl != null)
                    {
                        _customUrlService.Delete(customUrl);
                    }

                    _sectionService.Update(section);
                }
                //TODO: remove (temporary for output cache)
                MemoryCacheHelper.SectionsUpdateTime = DateTime.Now;
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
            if (!_customUrlService.IsUniquePath(section.Path))
            {
                ModelState.AddModelError("Path", "Путь не уникален");
            }
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<SectionVm, Section>(section);
                model.SectionState = SectionState.Active;
                model = _sectionService.Create(model);

                if (!string.IsNullOrEmpty(section.Path))
                {
                    var customUrlModel = new CustomUrl();
                    customUrlModel.Url = model.Path;
                    customUrlModel.ContentType = ContentType.Section;
                    customUrlModel.ContentId = model.Id;
                    _customUrlService.Create(customUrlModel);
                    //TODO: remove (temporary for output cache)
                    MemoryCacheHelper.SectionsUpdateTime = DateTime.Now;
                }

                return RedirectToAction("Index");
            }
            return View(section);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var section = _sectionService.GetById(id);
            SectionVm vm = Mapper.Map<Section, SectionVm>(section);

            var customUrl = _customUrlService.GetAll().FirstOrDefault(x => x.ContentId == vm.Id && x.ContentType == ContentType.Section);
            vm.Path = customUrl != null ? customUrl.Url : String.Empty;

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(SectionVm section)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<SectionVm, Section>(section);
                model.SectionState = SectionState.Active;

                var customUrl =
                    _customUrlService.GetAll()
                        .FirstOrDefault(x => x.ContentId == section.Id && x.ContentType == ContentType.Section);

                if (customUrl == null && !string.IsNullOrEmpty(section.Path))
                {
                    customUrl = new CustomUrl();
                }

                if (customUrl != null && customUrl.Url != section.Path)
                {
                    if (!_customUrlService.IsUniquePath(section.Path))
                    {
                        ModelState.AddModelError("Path", "Путь не уникален");
                        return View(section);
                    }

                    customUrl.Url = section.Path;
                    customUrl.ContentType = ContentType.Section;
                    customUrl.ContentId = section.Id;

                    _customUrlService.CreateOrUpdate(customUrl);
                }

                _sectionService.Update(model);
                //TODO: remove (temporary for output cache)
                MemoryCacheHelper.SectionsUpdateTime = DateTime.Now;

                return RedirectToAction("Index");
            }
            return View(section);
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
            private readonly ICustomUrlService _customUrlService;

            internal FakeSectionRepository(ISectionService sectionService, ICustomUrlService customUrlService)
            {
                _sectionService = sectionService;
                _customUrlService = customUrlService;
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
                var mapped = query.Select(Mapper.Map<Section, SectionRowVm>).ToList();

                foreach (var sectionVm in mapped)
                {
                    var customUrl = _customUrlService.GetAll().FirstOrDefault(x => x.ContentId == sectionVm.Id && x.ContentType == ContentType.Section);
                    sectionVm.Path = customUrl != null ? customUrl.Url : String.Empty;
                }

                return mapped;
            }
        }

        #endregion
    }
}