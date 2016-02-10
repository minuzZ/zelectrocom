using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZelectroCom.Web.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace ZelectroCom.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
