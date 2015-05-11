using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZelectroCom.Web.Startup))]
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
