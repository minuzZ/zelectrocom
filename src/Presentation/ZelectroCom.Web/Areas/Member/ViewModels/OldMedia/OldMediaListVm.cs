using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.OldMedia
{
    public class OldMediaListVm
    {
        [BsGrid(HasDetails = false, Theme = BsTheme.Green)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "OldMediaListVm_Grid")]
        
        public BsGridModel<OldMediaRowVm> Grid { get; set; }
    }
}