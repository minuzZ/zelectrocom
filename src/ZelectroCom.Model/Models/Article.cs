using System;
using System.ComponentModel.DataAnnotations;

namespace ZelectroCom.Data.Models
{
    public enum ArticleState
    {
        [Display(Name = "Пустой черновик")]
        New,
        [Display(Name = "Черновик статьи")]
        Draft,
        [Display(Name = "Отправлен на публикацию")]
        Posted,
        [Display(Name = "Возвращен на доработку")]
        ReturnedDraft,
        [Display(Name = "Опубликованная статья")]
        Article,
        [Display(Name = "Черновик удален")]
        Deleted
    }

    public class Article : AuditableEntity
    {
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public int? SectionId { get; set; }
        public Section Section { get; set; }

        public ArticleState ArticleState { get; set; }
        public DateTime PublishTime { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoUrl { get; set; }
        public string IndexHtml { get; set; }
        public int ViewsCount { get; set; }
        public int Rating { get; set; }
        public bool Hidden { get; set; }
    }
}
