using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class DraftRowVm : BsItemModel
    {
        public int Id { get; set; }

        [BsGridColumn(Width = 3, MediumWidth = 3)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "DraftRowVm_Title")]
        public string Title { get; set; }

        [BsGridColumn(Width = 2, MediumWidth = 2)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftRowVm_Section")]
        public string Section { get; set; }

        [BsGridColumn(Width = 2, MediumWidth = 2)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftRowVm_ArticleState")]
        public string ArticleState { get; set; }

        [BsGridColumn(Width = 2, MediumWidth = 2)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftRowVm_CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [BsGridColumn(Width = 2, MediumWidth = 2)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftRowVm_UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [BsGridColumn(Width = 1, MediumWidth = 1)]
        public string Action { get; set; }

        public override object GetUniqueID()
        {
            return Id;
        }
    }
}