using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DynTech.IdentityServer.Models;

namespace DynTech.IdentityServer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public async void EnsureSeedData(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            if (!this.Users.Any(u => u.UserName == "updateservice@magicsoft-asia.com"))
            {
                // Add 'admin' role
                var adminRole = await roleMgr.FindByNameAsync("admin");
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("admin");
                    await roleMgr.CreateAsync(adminRole);
                }

                // Add 'user' role
                var userRole = await roleMgr.FindByNameAsync("user");
                if (userRole == null)
                {
                    userRole = new IdentityRole("user");
                    await roleMgr.CreateAsync(userRole);
                }

                // create admin user
                var adminUser = new ApplicationUser();
                adminUser.UserName = "updateservice@magicsoft-asia.com";
                adminUser.Email = "updateservice@magicsoft-asia.com";

                await userMgr.CreateAsync(adminUser, "DynTech@82632");

                await userMgr.SetLockoutEnabledAsync(adminUser, false);
                await userMgr.AddToRoleAsync(adminUser, "admin");
            }
        }
    }
}
