using Autofac;
using ZelectroCom.Data;

namespace ZelectroCom.Web.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AppDbContext)).As(typeof(IContext)).InstancePerLifetimeScope();
        }
    }
}