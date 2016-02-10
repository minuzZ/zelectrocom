using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZelectroCom.Data.Models
{
    public enum SectionState
    {
        [Display(Name = "Активный раздел")]
        Active,
        [Display(Name = "Удален")]
        Deleted
    }

    public class Section : Entity
    {
        public virtual ICollection<Article> Articles { get; set; }
        public string Name { get; set; }

        public bool IsHidden { get; set; }

        public int Order { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeywords { get; set; }

        public SectionState SectionState { get; set; }
    }
}
