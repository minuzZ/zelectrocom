using System.Collections.Generic;
using ZelectroCom.Web.ViewModels.Home;

namespace ZelectroCom.Web.ViewModels.ZDev
{
    public class ZItemsListVm
    {
        public string UrlNext { get; set; }
        public string UrlBack { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public IEnumerable<ZDevIndexVm> ProductsList { get; set; }
    }
}