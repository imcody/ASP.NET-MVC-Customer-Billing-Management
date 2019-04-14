using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Billing.Web.Startup))]
namespace Billing.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
