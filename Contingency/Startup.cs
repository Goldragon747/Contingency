using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Contingency.Startup))]
namespace Contingency
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
