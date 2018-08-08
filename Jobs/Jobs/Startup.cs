using Jobs.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jobs.Startup))]
namespace Jobs
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUser();
        }
        public void CreateUser()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole identityRole = new IdentityRole();
            if (!roleManager.RoleExists("Admin"))
            {
                identityRole = new IdentityRole();
                identityRole.Name = "Admin";
                roleManager.Create(identityRole);

                var usermanger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Ramzi";
                user.Email = "rmazi@hotmail.com";
                              var check = usermanger.Create(user, "Aa-123456");
                if (check.Succeeded)
                {
                    usermanger.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Authors"))
            {
                identityRole = new IdentityRole();
                identityRole.Name = "Authors";
                roleManager.Create(identityRole);
            }
            if (!roleManager.RoleExists("User"))
            {
                identityRole = new IdentityRole();
                identityRole.Name = "User";
                roleManager.Create(identityRole);
            }
           
        }
    }
}
