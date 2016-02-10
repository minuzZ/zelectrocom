using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class PublicationsVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "PublicationsVm_Grid")]
        public BsGridModel<PublicationRowVm> Grid { get; set; }

        [BsToolbar(Theme = BsTheme.Green)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "PublicationsVm_Grid")]
        public BsToolbarModel<PublicationsSearchVm> Toolbar { get; set; }
    }
}