using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Slin.Facturacion.Electronica.Web.Startup))]
namespace Slin.Facturacion.Electronica.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
