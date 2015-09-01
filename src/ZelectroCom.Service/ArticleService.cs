using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public class ArticleService : EntityService<Article>, IArticleService
    {
        private const string SortDirectionAsc = "asc";
        public ArticleService(IContext context) : base(context) { }
        public bool IsDraft(Article item)
        {
            return item.ArticleState == ArticleState.Draft ||
                   item.ArticleState == ArticleState.Posted;
        }
        public IEnumerable<Article> GetDrafts()
        {
            var drafts = GetAll().Where(x => IsDraft(x));
            return drafts;
        }
    }
}
