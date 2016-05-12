using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatUp.Startup))]
namespace ChatUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
