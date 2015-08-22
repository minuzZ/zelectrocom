using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Section
{
    public class SectionVM
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Путь")]
        public string Path { get; set; }

        [Display(Name = "Скрытый раздел")]
        public bool IsHidden { get; set; }

        public int Order { get; set; }
    }
}