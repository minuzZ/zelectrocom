using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;
using ZelectroCom.Data.Tools;

namespace ZelectroCom.Service
{
    public class ArticleService : EntityService<Article>, IArticleService
    {
        private const string SortDirectionAsc = "asc";
        public ArticleService(IContext context) : base(context) { }
        public bool IsDraft(Article item)
        {
            return item.ArticleState == ArticleState.Draft ||
                   item.ArticleState == ArticleState.ReturnedDraft;
        }

        public bool IsPublished(Article item)
        {
            return item.ArticleState == ArticleState.Article;
        }

        public bool IsPosted(Article item)
        {
            return item.ArticleState == ArticleState.Posted;
        }

        public IEnumerable<Article> GetDrafts(string authorId = null)
        {
            var drafts = GetAll().Where(IsDraft);
            if (authorId != null)
            {
                drafts = drafts.Where(x => x.AuthorId == authorId);
            }
            return drafts;
        }

        public IEnumerable<Article> GetPublished(string authorId = null)
        {
            var drafts = GetAll().Where(IsPublished);
            if (authorId != null)
            {
                drafts = drafts.Where(x => x.AuthorId == authorId);
            }
            return drafts;
        }

        public IEnumerable<Article> GetPosted()
        {
            var drafts = GetAll().Where(IsPosted);
            return drafts;
        }

        public IEnumerable<Article> SearchArticles(string searchRequest)
        {
            var fts = FtsInterceptor.Fts(searchRequest);
            var drafts = _context.Articles.Where(x => x.Title.Contains(fts) || x.Text.Contains(fts));
            return drafts;
        }

        public IEnumerable<Article> GetArticlesForPage<TKey>(int pageSize, int page, out bool isLastPage, out bool isFirstPage,
            Func<Article, TKey> orderByFunc, IEnumerable<Article> articles = null, bool isAscOrder = false, int? sectionId = null)
        {
            IEnumerable<Article> res;
            int rangeStart;
            int rangeLength;
            int pageNumber = page;

            isLastPage = isFirstPage = false;

            if (articles == null)
            {
                articles = GetPublished();
            }

            res = sectionId != null ? articles.Where(x => x.SectionId == sectionId) : articles;

            int postsCount = res.Count();

            if ((pageNumber*pageSize) > postsCount)
                return null;

            if (((pageNumber*pageSize) + pageSize) > postsCount)
            {
                isLastPage = true;
                rangeLength = postsCount%pageSize;
                rangeStart = postsCount - rangeLength;
            }
            else
            {
                rangeStart = (pageNumber*pageSize);
                rangeLength = pageSize;
            }

            isFirstPage = rangeStart == 0;

            if (isAscOrder)
            {
                return res.OrderBy(orderByFunc)
                    .Skip(rangeStart).Take(rangeLength);
            }
            return res.OrderByDescending(orderByFunc)
                .Skip(rangeStart).Take(rangeLength);
        }
    }
}
