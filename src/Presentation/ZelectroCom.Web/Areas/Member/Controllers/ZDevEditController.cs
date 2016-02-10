using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
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
using ZelectroCom.Web.Areas.Member.ViewModels.ZDev;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles="Admin")]
    public class ZDevEditController : Controller
    {
        private readonly IZDevService _zDevService;
        private readonly FakeZDevRepository _fakeZDevRepository;

        public ZDevEditController(IZDevService zDevService)
        {
            _zDevService = zDevService;
            _fakeZDevRepository = new FakeZDevRepository(zDevService);
        }

        public ActionResult Index()
        {
            var gridModel = _fakeZDevRepository.ToBsGridViewModel(new BsGridBaseRepositorySettings() { Page = 1, PageSize = 10 });

            var model = new ZDevListVm()
            {
                Grid = gridModel,
            };

            var options = new Dictionary<string, string>
            {
                {"pagerUrl",  Url.Action("Pager")},
                {"deleteUrl", Url.Action("Delete")},
                {"zItemUrl", Url.Action("ZItem")},
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
                var viewModel = _fakeZDevRepository.ToBsGridViewModel(settings, out count).Wrap<ZDevListVm>(x => x.Grid);

                html = this.BsRenderPartialView("_ZDevGrid", viewModel);
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
                    var zDev = _zDevService.GetById(item.Id);

                    if (User.IsInRole("Admin"))
                    {
                        zDev.ZDevState = ZDevState.Deleted;
                        _zDevService.Update(zDev);
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

        [HttpGet]
        public ActionResult ZItem(int id = -1)
        {
            ZDev model;
            if (id < 0)
            {
                model = new ZDev();
            }
            else
            {
                model = _zDevService.GetById(id);
            }

            ZDevVm vm = Mapper.Map<ZDev, ZDevVm>(model);

            var options = new Dictionary<string, string>
            {
                {"saveZItemUrl",  Url.Action("SaveZItem")},
                {"successUrl",  Url.Action("Index")}
            };

            RequireJsOptions.Add("index", options);

            ViewBag.SuccessText = "Сохранено!";

            return View(vm);
        }

        [HttpPost]
        [NoAntiForgeryCheck]
        public BsJsonResult SaveZItem(ZDevVm zItem)
        {
            const string imagesPath = "~/Content/Uploads/ZDev/";
            string errorText = null;
            ZDev model = null;

            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (zItem.ImageUpload != null && zItem.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(zItem.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Выберите GIF, JPG или PNG изображение.");
            }

            if (ModelState.IsValid)
            {
                var zDev = _zDevService.GetById(zItem.Id);
                model = Mapper.Map(zItem, zDev);
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
            _zDevService.CreateOrUpdate(model);

            if (zItem.ImageUpload != null && zItem.ImageUpload.ContentLength > 0)
            {
                string folderPath = Server.MapPath(imagesPath);
                string imageName = model.Id + Path.GetExtension(zItem.ImageUpload.FileName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                model.ImagePath = imagesPath + imageName;
                _zDevService.Update(model);
                zItem.ImageUpload.SaveAs(Path.Combine(folderPath, imageName));   
            }

            return new BsJsonResult(new { Status = BsResponseStatus.Success });
        }

        #region BForms
        class FakeZDevRepository : BsBaseGridRepository<ZDev, ZDevRowVm>
        {
            private readonly IZDevService _zDevService;

            internal FakeZDevRepository(IZDevService zDevService)
            {
                _zDevService = zDevService;
            }

            public override IQueryable<ZDev> Query()
            {
                var query = _zDevService.GetAll().Where(x => x.ZDevState == ZDevState.Active).AsQueryable();
                return query;
            }

            public override IOrderedQueryable<ZDev> OrderQuery(IQueryable<ZDev> query, BsGridBaseRepositorySettings gridSettings = null)
            {
                var ordered = orderedQueryBuilder.Order(query, x => x.OrderBy(y => y.Order));
                return ordered;
            }

            public override IEnumerable<ZDevRowVm> MapQuery(IQueryable<ZDev> query)
            {
                var mapped = query.Select(Mapper.Map<ZDev, ZDevRowVm>);
                return mapped;
            }
        }
        #endregion
    }
}