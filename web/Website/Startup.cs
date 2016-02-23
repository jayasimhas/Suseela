using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Informa.Web.Startup))]
namespace Informa.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
