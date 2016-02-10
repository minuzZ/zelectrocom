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
        private bool TryParseUrl(string fullUrl, out string path, out int page)
        {
            bool result = true;
            path = String.Empty;
            page = 0;
            if (string.IsNullOrEmpty(fullUrl))
                return false;
            string[] parts = fullUrl.Split('/');
            path = parts[0];

            if (parts.Length > 1)
            {
                result &= int.TryParse(parts[1], out page);
            }

            return result;
        }

        protected override IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            string controller = String.Empty;
            string action = String.Empty;
            int page;
            string path;
            CustomUrl customUrl = null;

            var fullUrl = ((string)requestContext.RouteData.Values["url"]).ToLower();

            if (TryParseUrl(fullUrl, out path, out page))
            {
                if (CustomUrlCache.CustomUrls.ContainsKey(path))
                {
                    customUrl = CustomUrlCache.CustomUrls[path];
                }
                else
                {
                    ICustomUrlService customUrlService = (ICustomUrlService)DependencyResolver.Current.GetService(typeof(ICustomUrlService));

                    customUrl = customUrlService.GetAll().FirstOrDefault(x => String.Equals(x.Url, path, StringComparison.CurrentCultureIgnoreCase));
                    if (customUrl != null)
                    {
                        CustomUrlCache.CustomUrls.TryAdd(path, customUrl);
                    }
                }

                if (customUrl != null)
                {
                    switch (customUrl.ContentType)
                    {
                        case ContentType.Article:
                            controller = "Post";
                            action = "Article";
                            break;
                        case ContentType.Section:
                            controller = "Home";
                            action = "Section";
                            requestContext.RouteData.Values["page"] = page;
                            break;
                        default:
                            throw new ArgumentException("Invalid ContentType");
                    }
                }
            }

            if (customUrl == null)
            {
                controller = "Common";
                action = "NoPageFound";
            }
            else
            {
                requestContext.RouteData.Values["id"] = customUrl.ContentId;
            }
            requestContext.RouteData.DataTokens.Add("namespaces", new string[] { "ZelectroCom.Web.Controllers" });
            requestContext.RouteData.Values["controller"] = controller;
            requestContext.RouteData.Values["action"] = action;

            return base.GetHttpHandler(requestContext);
        }
    }
}