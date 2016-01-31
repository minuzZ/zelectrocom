using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Profile
{
    public enum ProfilePanelComponentsEnum
    {
        UserProfile,
        UserData
    }

    [Serializable]
    public class ProfileVm
    {
        [BsPanel(Id = ProfilePanelComponentsEnum.UserProfile, Expandable = false, Editable = false)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ProfileVm_UserProfile")]
        public UserProfileVm UserProfile { get; set; }

        [BsPanel(Id = ProfilePanelComponentsEnum.UserData)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ProfileVm_UserInfo")]
        public UserDataVm UserData { get; set; }
    }

    [Serializable]
    public class UserProfileVm
    {
        public string Nickname { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }
    }

    [Serializable]
    public class UserDataVm
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Description { get; set; }
    }

    #region Editable
    public class EditableProfileVm
    {
        [BsPanel(Id = ProfilePanelComponentsEnum.UserData)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "ProfileVm_UserInfo")]
        public EditableUserDataVm UserData { get; set; }
    }

    public class EditableUserDataVm
    {
        [Display(ResourceType = typeof (Resources.Resources), Name = "EditableUserDataVm_Firstname")]
        [BsControl(BsControlType.TextBox)]
        public string Firstname { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "EditableUserDataVm_Lastname")]
        [BsControl(BsControlType.TextBox)]
        public string Lastname { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "EditableUserDataVm_Description")]
        [BsControl(BsControlType.TextBox)]
        public string Description { get; set; }
    }

    #endregion
}