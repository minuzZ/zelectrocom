using System.ComponentModel.DataAnnotations;
using BForms.Models;
using BForms.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.OldMedia
{
    public class OldMediaRowVm : BsItemModel
    {
        public int Id { get; set; }
        
        [BsGridColumn(Width = 6, MediumWidth = 6)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "OldMediaRowVm_OldPath")]
        public string OldPath { get; set; }

        [BsGridColumn(Width = 6, MediumWidth = 6)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "OldMediaRowVm_NewPath")]
        public string NewPath { get; set; }

        public override object GetUniqueID()
        {
            return Id;
        }
    }
}