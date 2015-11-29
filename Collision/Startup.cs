using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Collision.Startup))]
namespace Collision
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
