using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.ZDev
{
    public class ZDevListVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevListVm_Grid")]
        public BsGridModel<ZDevRowVm> Grid { get; set; }
    }
}