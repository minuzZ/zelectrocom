using System.Reflection;
using Autofac;

namespace ZelectroCom.Web.Modules
{
    public class HelperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("ZelectroCom.Web"))
                .Where(t => t.Namespace == "ZelectroCom.Web.Infrastructure.Helpers")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}