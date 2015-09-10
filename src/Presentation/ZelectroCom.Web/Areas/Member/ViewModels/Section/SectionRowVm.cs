using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Section
{
    public class SectionRowVm : BsItemModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength()]
        [BsGridColumn(Width = 4, MediumWidth = 4)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "SectionVm_Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [BsGridColumn(Width = 4, MediumWidth = 4)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "SectionVm_Path")]
        public string Path { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "SectionVm_IsHidden")]
        [BsGridColumn(Width = 2, MediumWidth = 2)]
        public bool IsHidden { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "SectionVm_Order")]
        [BsGridColumn(Width = 1, MediumWidth = 1)]
        public int Order { get; set; }

        [BsGridColumn(Width = 1, MediumWidth = 1)]
        public string Action { get; set; }

        public override object GetUniqueID()
        {
            return Id;
        }
    }
}