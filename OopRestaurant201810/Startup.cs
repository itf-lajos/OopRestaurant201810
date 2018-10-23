using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OopRestaurant201810.Startup))]
namespace OopRestaurant201810
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
