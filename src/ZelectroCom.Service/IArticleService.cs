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
        IEnumerable<Article> GetDrafts(); 
        bool IsDraft(Article item);
    }
}
