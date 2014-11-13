using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MC3Shopper.Startup))]

namespace MC3Shopper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}