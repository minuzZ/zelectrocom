using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelectroCom.Web.Areas.Member.ViewModels.Article
{
    public class DraftListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public string ArticleState { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}