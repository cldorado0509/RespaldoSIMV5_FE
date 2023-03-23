using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIM.Startup))]
namespace SIM
{
    public partial class Startup
    {
        //Cambios de prueba Git
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
