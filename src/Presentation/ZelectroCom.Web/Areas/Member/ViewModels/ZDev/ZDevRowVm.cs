using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ZelectroCom.Web.Areas.Member.ViewModels.ZDev
{
    public class ZDevRowVm : BsItemModel
    {
        public int Id { get; set; }

        [BsGridColumn(Width = 6, MediumWidth = 6)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "ZDevVm_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "ZDevVm_Order")]
        [BsGridColumn(Width = 5, MediumWidth = 5)]
        public int Order { get; set; }

        [BsGridColumn(Width = 1, MediumWidth = 1)]
        public string Action { get; set; }

        public override object GetUniqueID()
        {
            return Id;
        }
    }
}