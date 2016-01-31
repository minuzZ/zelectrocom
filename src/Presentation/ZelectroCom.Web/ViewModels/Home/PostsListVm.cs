using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelectroCom.Web.ViewModels.Home
{
    public class PostsListVm
    {
        public string ScrollUrl { get; set; }
        public IEnumerable<PostIndexVm> PostsList { get; set; }
    }
}