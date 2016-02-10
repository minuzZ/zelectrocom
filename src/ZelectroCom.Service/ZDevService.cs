using System.Collections.Generic;
using System.Linq;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;

namespace ZelectroCom.Service
{
    public class ZDevService : EntityService<ZDev>, IZDevService
    {
        public ZDevService(IContext context) : base(context) { }

        public IEnumerable<ZDev> GetProductsForPage(int pageSize, int page, out bool isLastPage, out bool isFirstPage)
        {
            IEnumerable<ZDev> res = GetAll().Where(x => x.ZDevState == ZDevState.Active);
            int rangeStart;
            int rangeLength;
            int pageNumber = page;

            isLastPage = isFirstPage = false;

            int postsCount = res.Count();

            if ((pageNumber * pageSize) > postsCount)
                return null;

            if (((pageNumber * pageSize) + pageSize) > postsCount)
            {
                isLastPage = true;
                rangeLength = postsCount % pageSize;
                rangeStart = postsCount - rangeLength;
            }
            else
            {
                rangeStart = (pageNumber * pageSize);
                rangeLength = pageSize;
            }

            isFirstPage = rangeStart == 0;

            return res.OrderBy(x => x.Order)
                .Skip(rangeStart).Take(rangeLength);
        }
    }
}
