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
        IEnumerable<Article> GetDrafts(int? page, int? limit, string sortBy, string direction,
            string searchString, out int total); 
        bool IsDraft(Article item);
    }
}
