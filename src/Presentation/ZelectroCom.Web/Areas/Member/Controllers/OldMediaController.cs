using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BForms.Grid;
using BForms.Models;
using BForms.Mvc;
using Microsoft.VisualBasic.FileIO;
using RequireJsNet;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Areas.Member.ViewModels.OldMedia;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    public class OldMediaController : Controller
    {
        private readonly IOldMediaService _oldMediaService;
        private readonly FakeOldMediaRepository _fakeOldMediaRepository;
        public OldMediaController(IOldMediaService oldMediaService)
        {
            _oldMediaService = oldMediaService;
            _fakeOldMediaRepository = new FakeOldMediaRepository(_oldMediaService);
        }
        public ActionResult Index()
        {
            var gridModel =
                _fakeOldMediaRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 20 });

            var model = new OldMediaListVm()
            {
                Grid = gridModel
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl", Url.Action("Pager")}
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
                var viewModel = _fakeOldMediaRepository.ToBsGridViewModel(settings, out count).Wrap<OldMediaListVm>(x => x.Grid);

                html = this.BsRenderPartialView("_OldMediaGrid", viewModel);
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

        [HttpPost]
        public ActionResult ImportCsv(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            try
            {
                _oldMediaService.Clear();
                using (
                    TextFieldParser parser = new TextFieldParser(file.InputStream)
                    {
                        TextFieldType = FieldType.Delimited
                    })
                {
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        string oldPath = getOldPath(fields[0]);
                        string newPath = getNewPath(fields[1]);

                        _oldMediaService.Create(new OldMedia() { OldPath = oldPath, NewPath = newPath });
                    }
                    parser.Close();
                }
            }
            catch (Exception ex)
            {
                //TODO: log and display
            }
            return RedirectToAction("Index");
        }

        private string getNewPath(string p)
        {
            if (!p.StartsWith("/"))
            {
                return "/" + p;
            }
            return p;
        }

        private string getOldPath(string p)
        {
            if (p.Contains("Media/Default/"))
            {
                return p.Substring(p.LastIndexOf("Default/", StringComparison.Ordinal) + "Default/".Length);
            }
            return p;
        }

        public ActionResult ExportCsv()
        {
            var allRecords = _oldMediaService.GetAll().AsEnumerable();
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            foreach (OldMedia record in allRecords)
            {
                writer.Write(record.OldPath);
                writer.Write(",");
                writer.Write(record.NewPath);
                writer.Write(Environment.NewLine);
            }
            writer.Flush();
            memoryStream.Position = 0;

            return File(memoryStream, "text/csv", "redirects.csv");
        }

        #region BForms

        private class FakeOldMediaRepository : BsBaseGridRepository<OldMedia, OldMediaRowVm>
        {
            private readonly IOldMediaService _oldMediaService;

            internal FakeOldMediaRepository(IOldMediaService oldMediaService)
            {
                _oldMediaService = oldMediaService;
            }

            public override IQueryable<OldMedia> Query()
            {
                var oldMediaQuery = _oldMediaService.GetAll().AsQueryable();
                return oldMediaQuery;
            }

            public override IOrderedQueryable<OldMedia> OrderQuery(IQueryable<OldMedia> query,
                BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.Id));
                return ordered;
            }

            public override IEnumerable<OldMediaRowVm> MapQuery(IQueryable<OldMedia> query)
            {
                var mapped = query.Select(Mapper.Map<OldMedia, OldMediaRowVm>).ToList();
                return mapped;
            }
        }

        #endregion
    }
}