using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;

namespace ZelectroCom.Web.Infrastructure
{
    /// <summary>
    /// Temporary solution to prevent multiple db calls when retrieving custom urls
    /// </summary>
    public static class CustomUrlCache
    {
        private static ConcurrentDictionary<string, CustomUrl> CustomUrlDict = new ConcurrentDictionary<string, CustomUrl>();
        public static ConcurrentDictionary<string, CustomUrl> CustomUrls { get { return CustomUrlDict; } }

        public static void RefreshCache()
        {
            CustomUrls.Clear();
        }
    }

    public class CustomUrlRouteHandler : System.Web.Mvc.MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            string controller = String.Empty;
            string action = String.Empty;
            string id = String.Empty;

            CustomUrl customUrl = null;

            var url = ((string)requestContext.RouteData.Values["url"]).ToLower();

            if (CustomUrlCache.CustomUrls.ContainsKey(url))
            {
                customUrl = CustomUrlCache.CustomUrls[url];
            }
            else
            {
                ICustomUrlService customUrlService = (ICustomUrlService)DependencyResolver.Current.GetService(typeof(ICustomUrlService));

                customUrl = customUrlService.GetAll().FirstOrDefault(x => String.Equals(x.Url, url, StringComparison.CurrentCultureIgnoreCase));
                if (customUrl != null)
                {
                    CustomUrlCache.CustomUrls.TryAdd(url, customUrl);
                }
            }

            if (customUrl != null)
            {
                switch (customUrl.ContentType)
                {
                    case ContentType.Article:
                        controller = "Post";
                        action = "Article";
                        id = customUrl.ContentId.ToString();
                        break;
                    case ContentType.Section:
                        controller = "Home";
                        action = "Section";
                        id = customUrl.ContentId.ToString();
                        break;
                    default:
                        throw new ArgumentException("Invalid ContentType");
                }
            }

            /*TODO: 404*/

            requestContext.RouteData.DataTokens.Add("namespaces", new string[] { "ZelectroCom.Web.Controllers" });
            requestContext.RouteData.Values["controller"] = controller;
            requestContext.RouteData.Values["action"] = action;
            requestContext.RouteData.Values["id"] = id;

            return base.GetHttpHandler(requestContext);
        }
    }
}