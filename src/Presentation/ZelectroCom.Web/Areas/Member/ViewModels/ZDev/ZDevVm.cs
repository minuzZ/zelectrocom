using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using BForms.Models;
using BForms.Mvc;
using Newtonsoft.Json;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Web.Areas.Member.ViewModels.ZDev
{
    public class ZDevVm
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Validation_RequiredField", ErrorMessageResourceType = typeof(Resources.Resources))]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevVm_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevVm_ImagePath")]
        [BsControl(BsControlType.TextBox)]
        //[DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageUpload { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevVm_Description")]
        [BsControl(BsControlType.TextBox)]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "Validation_RequiredField", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevVm_Url")]
        [BsControl(BsControlType.TextBox)]
        public string Url { get; set; }

        [Required(ErrorMessageResourceName = "Validation_RequiredField", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ZDevVm_Order")]
        [BsControl(BsControlType.NumberInline)]
        public BsRangeItem<int> Order { get; set; }
    }
}