using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TImeDesk.Startup))]
namespace TImeDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
