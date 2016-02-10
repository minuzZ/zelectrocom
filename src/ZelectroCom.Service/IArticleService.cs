using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public interface IArticleService : IEntityService<Article>
    {
        IEnumerable<Article> GetDrafts(string authorId = null);
        IEnumerable<Article> GetPublished(string authorId = null);
        IEnumerable<Article> GetPosted();
        IEnumerable<Article> SearchArticles(string searchText);

        IEnumerable<Article> GetArticlesForPage<TKey>(int pagesize, int page, out bool isLastPage, out bool isFirstPage,
            Func<Article, TKey> orderByFunc, IEnumerable<Article> articles = null, 
            bool isAscOrder = false, int? sectionId = null);

        bool IsDraft(Article item);
        bool IsPublished(Article item);
        bool IsPosted(Article item);
    }
}
