using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class DraftListVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(Name = "DraftListVm_Drafts", ResourceType = typeof(Resources.Resources))]
        public BsGridModel<DraftRowVm> Grid { get; set; }
    }
}