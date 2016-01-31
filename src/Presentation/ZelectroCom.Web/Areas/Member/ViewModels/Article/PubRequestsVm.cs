using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class PubRequestsVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "PubRequestsVm_Grid")]
        public BsGridModel<PubRequestRowVm> Grid { get; set; }
    }
}