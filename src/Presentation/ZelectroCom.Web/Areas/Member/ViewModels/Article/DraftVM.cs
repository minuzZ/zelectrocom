using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class DraftVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Раздел")]
        public int SectionId { get; set; }
        [Display(Name = "Дата и время публикации")]
        public DateTime PublishTime { get; set; }
        public SelectList Sections { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        [Display(Name = "SEO заголовок")]
        public string SeoTitle { get; set; }
        [Display(Name = "SEO описание")]
        public string SeoDescription { get; set; }
        [Display(Name = "SEO ключевые слова")]
        public string SeoKeywords { get; set; }
        [Display(Name = "SEO URL")]
        public string SeoUrl { get; set; }
    }
}