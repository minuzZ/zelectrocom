using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BForms.Mvc;
using ZelectroCom.Web.Infrastructure;
using ZelectroCom.Web.Infrastructure.Helpers;
using ZelectroCom.Web.Modules;

namespace ZelectroCom.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterModule(new HelperModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Automapper
            AutoMapperConfiguration.Configure();

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //register BForms validation provider
            ModelValidatorProviders.Providers.Add(new BsModelValidatorProvider());
            BForms.Utilities.BsResourceManager.Register(Resources.Resources.ResourceManager);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            _logger.Error("GLOBAL> ", exception);
            Server.ClearError();
            Response.Redirect("/Home/Error");
        }

        //TODO: remove (temporary for output cache)
        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.Equals("SectionsUpdate", StringComparison.InvariantCultureIgnoreCase))
            {
                return MemoryCacheHelper.SectionsUpdateTime.ToString(CultureInfo.InvariantCulture);
            }
            if (arg.Equals("ArticlesUpdate", StringComparison.InvariantCultureIgnoreCase))
            {
                return MemoryCacheHelper.ArticlesUpdateTime.ToString(CultureInfo.InvariantCulture);
            }
            if (arg.Equals("SectionsArticlesUpdate", StringComparison.InvariantCultureIgnoreCase))
            {
                return MemoryCacheHelper.ArticlesUpdateTime.ToString(CultureInfo.InvariantCulture) + MemoryCacheHelper.SectionsUpdateTime.ToString(CultureInfo.InvariantCulture);
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }
}
