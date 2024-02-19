using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCBlog.Website.Startup))]
namespace MVCBlog.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
