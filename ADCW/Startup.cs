using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADCW.Startup))]
namespace ADCW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
