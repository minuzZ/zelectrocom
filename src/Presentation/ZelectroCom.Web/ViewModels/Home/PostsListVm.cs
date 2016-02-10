using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelectroCom.Web.ViewModels.Home
{
    public class PostsListVm
    {
        public string UrlNext { get; set; }
        public string UrlBack { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public IEnumerable<PostIndexVm> PostsList { get; set; }
    }
}