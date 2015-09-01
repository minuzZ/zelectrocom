using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class DraftVm
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Validation_RequiredField", ErrorMessageResourceType = typeof(Resources.Resources))]
        [MaxLength(50, ErrorMessageResourceName = "Validation_Length50", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(ResourceType = typeof(Resources.Resources), Name = "DraftVm_Title", Prompt = "DraftVm_Title_Prompt")]
        [BsControl(BsControlType.TextBox)]
        public string Title { get; set; }
        
        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftVm_PublishTime")]
        [BsControl(BsControlType.DatePicker)]
        public BsDateTime PublishTime { get; set; }

        [Required(ErrorMessageResourceName = "Validation_RequiredField", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(ResourceType = typeof(Resources.Resources), Name = "DraftVm_SectionId", Prompt = "DraftVm_SectionId_Prompt")]
        [BsControl(BsControlType.DropDownList)]
        public int SectionId { get; set; }
        public SelectList Sections { get; set; }

        [AllowHtml]
        [BsControl(BsControlType.TextArea)]
        public string Text { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftVm_SeoTitle")]
        [BsControl(BsControlType.TextBox)]
        public string SeoTitle { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftVm_SeoDescription")]
        [BsControl(BsControlType.TextArea)]
        public string SeoDescription { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftVm_SeoKeywords")]
        [BsControl(BsControlType.TextBox)]
        public string SeoKeywords { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DraftVm_SeoUrl")]
        [BsControl(BsControlType.TextBox)]
        public string SeoUrl { get; set; }
    }
}