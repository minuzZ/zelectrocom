using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelectroCom.Web.ViewModels.Home
{
    public class PostIndexVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishTime { get; set; }
        public string AuthorName { get; set; }
        public string SeoUrl { get; set; }
        public string IndexHtml { get; set; }
    }
}