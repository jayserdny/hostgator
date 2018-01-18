using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frogmire.Startup))]
namespace Frogmire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
