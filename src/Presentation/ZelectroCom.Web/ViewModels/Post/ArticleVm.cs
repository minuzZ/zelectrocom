﻿using System;

namespace ZelectroCom.Web.ViewModels.Post
{
    public class ArticleVm
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime PublishTime { get; set; }

        public string Text { get; set; }

        public string AuthorName { get; set; }
    }
}