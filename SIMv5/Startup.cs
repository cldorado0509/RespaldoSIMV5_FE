using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIM.Startup))]
namespace SIM
{
    public partial class Startup
    {//Version 14
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
