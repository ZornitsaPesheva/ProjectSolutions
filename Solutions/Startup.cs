using Microsoft.Owin;
using Owin;

using System.Data.Entity;
using Solutions.Migrations;
using Solutions.Models;

[assembly: OwinStartupAttribute(typeof(Solutions.Startup))]
namespace Solutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
