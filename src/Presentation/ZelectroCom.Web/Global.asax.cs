using System.Resources;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BForms.Mvc;
using ZelectroCom.Web.Infrastructure;
using ZelectroCom.Web.Modules;

namespace ZelectroCom.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new AutofacWebTypesModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Automapper
            AutoMapperConfiguration.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //register BForms validation provider
            ModelValidatorProviders.Providers.Add(new BsModelValidatorProvider());
            BForms.Utilities.BsResourceManager.Register(Resources.Resources.ResourceManager);
        }
    }
}
