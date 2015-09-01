using System;
using System.ComponentModel.DataAnnotations;

namespace ZelectroCom.Web.ViewModels
{
    public class PreviewArticleVm
    {
        public string Title { get; set; }

        public DateTime PublishTime { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }
    }
}