using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZelectroCom.Web.ViewModels.Section
{
    public class SectionVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public int Page { get; set; }
    }
}