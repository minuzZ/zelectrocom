using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class PublicationsSearchVm
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "DraftRowVm_Title")]
        [BsControl(BsControlType.TextBox)]
        public string Title { get; set; }
    }
}