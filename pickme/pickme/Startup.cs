using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pickme.Startup))]
namespace pickme
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
