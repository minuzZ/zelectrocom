using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using BForms.Models;
using BForms.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RequireJsNet;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;
using ZelectroCom.Web.Areas.Member.ViewModels.Profile;
using ZelectroCom.Web.Infrastructure;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ProfileController : Controller
    {
        protected AppDbContext ApplicationDbContext { get; set; }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ProfileController()
        {
            this.ApplicationDbContext = new AppDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public ActionResult Index()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            ProfileVm vm = Mapper.Map<ApplicationUser, ProfileVm>(user);

            RequireJsOptions.Add("uploadUrl", Url.Action("UploadAvatar"));
            RequireJsOptions.Add("avatarUrl", Url.Action("GetAvatar"));
            RequireJsOptions.Add("deleteAvatarUrl", Url.Action("DeleteAvatar"));

            return View(vm);
        }

        [HttpPost]
        [NoAntiForgeryCheck]
        public BsJsonResult GetReadonlyContent(ProfilePanelComponentsEnum componentId)
        {
            var html = string.Empty;
            var msg = string.Empty;
            var status = BsResponseStatus.Success;

            try
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                ProfileVm vm = Mapper.Map<ApplicationUser, ProfileVm>(user);

                switch (componentId)
                {
                    case ProfilePanelComponentsEnum.UserProfile:
                        html = this.BsRenderPartialView("Readonly/_UserProfile", vm.UserProfile);
                        break;
                    case ProfilePanelComponentsEnum.UserData:
                        html = this.BsRenderPartialView("Readonly/_UserData", vm.UserData);
                        break;
                }

            }

            catch (Exception ex)
            {
                msg = ex.Message;
                status = BsResponseStatus.ServerError;
            }

            return new BsJsonResult(new
            {
                Html = html
            }, status, msg);
        }

        [HttpPost]
        [NoAntiForgeryCheck]
        public BsJsonResult GetEditableContent(ProfilePanelComponentsEnum componentId)
        {
            var html = string.Empty;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            EditableProfileVm vm = Mapper.Map<ApplicationUser, EditableProfileVm>(user);

            switch (componentId)
            {
                case ProfilePanelComponentsEnum.UserData:
                    html = this.BsRenderPartialView("Editable/_UserData", vm.UserData, vm.GetPropertyName(x => x.UserData));
                    break;
            }

            return new BsJsonResult(new
            {
                Html = html
            });
        }

        [HttpPost]
        [NoAntiForgeryCheck] //???
        public BsJsonResult SetContent(EditableProfileVm model, ProfilePanelComponentsEnum componentId)
        {

            var html = string.Empty;
            var status = BsResponseStatus.Success;
            var msg = string.Empty;
            UserDataVm userDataVm = null;

            switch (componentId)
            {
                case ProfilePanelComponentsEnum.UserData:
                    ModelState.ClearModelState(model.GetPropertyName(m => m.UserData) + ".");
                    break;
            }

            try
            {

                if (ModelState.IsValid)
                {
                    ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

                    switch (componentId)
                    {
                        case ProfilePanelComponentsEnum.UserData:
                            user = Mapper.Map(model.UserData, user);
                            UserManager.Update(user);

                            userDataVm = Mapper.Map<ApplicationUser, UserDataVm>(user);

                            html = this.BsRenderPartialView("Readonly/_UserData", userDataVm);
                            break;
                    }


                }
                else
                {
                    //JSON serialize ModelState errors
                    return new BsJsonResult(
                        new Dictionary<string, object> { { "Errors", ModelState.GetErrors() } },
                        BsResponseStatus.ValidationError);
                }

            }

            catch (Exception ex)
            {
                msg = "<strong>" + Resources.Resources.Error_Server + "!</strong> " + ex.Message;
                status = BsResponseStatus.ServerError;
            }

            return new BsJsonResult(new
            {
                Html = html,
                Profile = userDataVm
            }, status, msg);
        }
	}
}