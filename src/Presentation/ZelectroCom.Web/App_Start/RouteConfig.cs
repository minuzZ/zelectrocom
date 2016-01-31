using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZelectroCom.Web.Infrastructure;

namespace ZelectroCom.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "OldMedia",
                url: "Media/Default/{*url}",
                defaults: new { controller = "OldMedia", action = "Index"},
                namespaces: new[] { "ZelectroCom.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = "Account|Common|Home|Post|Search|Section" },
                namespaces: new[] { "ZelectroCom.Web.Controllers"}
            );

            routes.MapRoute(
                name: "CatchAll",
                url: "{*url}",
                defaults: null,
                constraints: new { url = "^(?!(Content|bundles)).*$" } 
            ).RouteHandler = new CustomUrlRouteHandler();
        }
    }
}
