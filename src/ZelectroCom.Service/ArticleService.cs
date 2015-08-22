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
        public IEnumerable<Article> GetDrafts(int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            var records = GetAll().Where(x => IsDraft(x)).AsQueryable();

            total = records.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Title.ToLower().Contains(searchString.ToLower()));
            }

            records = records.OrderBy(x => x.CreatedDate);

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            return records.ToList();
        }
    }
}
