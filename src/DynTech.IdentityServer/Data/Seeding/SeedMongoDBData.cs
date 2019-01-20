using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DynTech.IdentityServer.Configuration;
using DynTech.IdentityServer.Models;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using identityCore = Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using IdentityServer4.Models;
using System;

namespace DynTech.IdentityServer.Data.Seeding
{
    /// <summary>
    /// Seed mongo DBD ata.
    /// </summary>
    public class SeedMongoDBData
    {
        /// <summary>
        /// Seeding data.
        /// </summary>
        /// <param name="serviceScope">service Scope.</param>
        public static void Seeding(IServiceScope serviceScope)
        {
            var seeding = new SeedMongoDBData();
            var provider = serviceScope.ServiceProvider;
            var context = provider.GetService<IConfigurationDbContext>();
            seeding.EnsureSeedData(context).Wait();

            var userManager = provider.GetService<identityCore.UserManager<ApplicationUser>>();
            var roleManager = provider.GetService<identityCore.RoleManager<IdentityRole>>();

            seeding.EnsureSuperUser(userManager, roleManager).Wait();
        }

        /// <summary>
        /// Ensures the seed data.
        /// </summary>
        /// <param name="context">Context.</param>
        private async Task EnsureSeedData(IConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any())
            {
                await context.AddIdentityResource((new IdentityResources.OpenId()).ToEntity());
                await context.AddIdentityResource((new IdentityResources.Profile()).ToEntity());
                await context.AddIdentityResource((new IdentityResources.Email()).ToEntity());
                await context.AddIdentityResource((new IdentityResources.Phone()).ToEntity());
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Configuration.Resources.GetApiResources().ToList())
                {
                    await context.AddApiResource(resource.ToEntity());
                }
            }
        }

        /// <summary>
        /// Ensures super user account
        /// </summary>
        private async Task EnsureSuperUser(identityCore.UserManager<ApplicationUser> userManager, identityCore.RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SiteAdmin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("SiteAdmin");
                var addedRole = await roleManager.CreateAsync(adminRole);

                if (addedRole.Succeeded)
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "users.view"));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "users.create"));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "users.update"));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "clients.view"));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "idres.view"));
                    await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "apires.view"));
                }
            }

            var userRole = await roleManager.FindByNameAsync("SiteUser");
            if (userRole == null)
            {
                userRole = new IdentityRole("SiteUser");
                var addedRole = await roleManager.CreateAsync(userRole);

                if (addedRole.Succeeded)
                {
                    await roleManager.AddClaimAsync(userRole, new Claim(CustomClaimTypes.Permission, "clients.view"));
                    await roleManager.AddClaimAsync(userRole, new Claim(CustomClaimTypes.Permission, "clients.create"));
                    await roleManager.AddClaimAsync(userRole, new Claim(CustomClaimTypes.Permission, "clients.update"));
                }
            }

            var envVar = Environment.GetEnvironmentVariables();
            var init_admin_pwd = envVar["INIT_ADMIN_PWD"] != null ? envVar["INIT_ADMIN_PWD"].ToString() : string.Empty;

            var user = new ApplicationUser()
            {
                UserName = "idsrv_admin",
                Email = "idsrv_admin@dyntech.solutions",
                EmailConfirmed = true,
                IsAdmin = true,
                PasswordHash = init_admin_pwd,
            };

            var existing = await userManager.FindByEmailAsync(user.Email);
            if (existing == null)
            {
                adminRole = await roleManager.FindByNameAsync("SiteAdmin");
                user.AddRole(adminRole.Name);
                user.NormalizedUserName = user.UserName;

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    Log.Information("Created super user.");
                }

                await userManager.AddClaimsAsync(user, adminRole.Claims.Select(c => c.ToSecurityClaim()));
            }
            else
            {
                Log.Information("user {0} exists!", user.Email);
            }
        }
    }
}