using System.Web;
using System.Web.Mvc;
using ZelectroCom.Web.Infrastructure.Filters;

namespace ZelectroCom.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new RequireHttpsAttribute());
            filters.Add(new AntiForgeryFilter());
        }
    }
}
