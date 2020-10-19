using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WADAuth.Startup))]
namespace WADAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
