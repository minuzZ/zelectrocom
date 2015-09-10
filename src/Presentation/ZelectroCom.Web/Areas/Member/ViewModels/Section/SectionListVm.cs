using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Section
{
    public class SectionListVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "SectionListVm_Grid")]
        public BsGridModel<SectionRowVm> Grid { get; set; }
    }
}