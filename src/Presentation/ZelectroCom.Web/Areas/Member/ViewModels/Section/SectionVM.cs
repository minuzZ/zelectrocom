using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Section
{
    public class SectionVm
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "SectionVm_Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "SectionVm_Path")]
        public string Path { get; set; }

        [Required]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Description = "SectionVm_Description")]
        public string Description { get; set; }

        [Required]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Description = "SectionVm_SeoTitle")]
        public string SeoTitle { get; set; }

        [Required]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Description = "SectionVm_SeoDescription")]
        public string SeoDescription { get; set; }

        [Required]
        [BsControl(BsControlType.TextBox)]
        [Display(ResourceType = typeof (Resources.Resources), Description = "SectionVm_SeoKeywords")]
        public string SeoKeywords { get; set; }

        [BsControl(BsControlType.CheckBox)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "SectionVm_IsHidden")]
        public bool IsHidden { get; set; }

        [BsControl(BsControlType.NumberInline)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "SectionVm_Order")]
        public BsRangeItem<int> Order { get; set; }
    }
}