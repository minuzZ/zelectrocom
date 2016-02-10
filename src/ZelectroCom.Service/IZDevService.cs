using System.Collections.Generic;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public interface IZDevService : IEntityService<ZDev>
    {
        IEnumerable<ZDev> GetProductsForPage(int pagesize, int page, out bool isLastPage, out bool isFirstPage);
    }
}
