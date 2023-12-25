using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentLinkUp_MVC_.Startup))]
namespace StudentLinkUp_MVC_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
