using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VerticeSqlPoc.Web.Api.Startup))]
namespace VerticeSqlPoc.Web.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
