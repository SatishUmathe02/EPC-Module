using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EPCWeb.Startup))]
namespace EPCWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
