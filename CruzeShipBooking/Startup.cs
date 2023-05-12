using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CruzeShipBooking.Startup))]
namespace CruzeShipBooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
