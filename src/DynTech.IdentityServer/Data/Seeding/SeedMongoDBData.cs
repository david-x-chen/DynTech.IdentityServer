using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DynTech.IdentityServer.Configuration;
using DynTech.IdentityServer.Models;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
        public async Task Seeding(IServiceScope serviceScope)
        {
                var provider = serviceScope.ServiceProvider;
                EnsureSeedData(provider.GetService<IConfigurationDbContext>());

                var userManager = provider.GetService<UserManager<ApplicationUser>>();
                var roleManager = provider.GetService<RoleManager<IdentityRole>>();
                await EnsureSuperUser(userManager, roleManager);
        }

        /// <summary>
        /// Ensures the seed data.
        /// </summary>
        /// <param name="context">Context.</param>
        private void EnsureSeedData(IConfigurationDbContext context)
        {
            //if (!context.Clients.Any())
            //{
            //    foreach (var client in Clients.Get().ToList())
            //    {
            //        context.AddClient(client.ToEntity());
            //    }
            //}

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.GetIdentityResources().ToList())
                {
                    context.AddIdentityResource(resource.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.GetApiResources().ToList())
                {
                    context.AddApiResource(resource.ToEntity());
                }
            }
        }

        /// <summary>
        /// Ensures super user account
        /// </summary>
        private async Task EnsureSuperUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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

            var user = new ApplicationUser()
            {
                UserName = "idsrv_admin",
                Email = "idsrv_admin@dyntech.solutions",
                EmailConfirmed = true,
                IsAdmin = true
            };

            adminRole = await roleManager.FindByNameAsync("SiteAdmin");
            user.AddRole(adminRole.Name);

            var result = await userManager.CreateAsync(user, "P@$sw0rd");
            if (result.Succeeded)
            {
                Log.Information("Created super user.");
            }

            await userManager.AddClaimsAsync(user, adminRole.Claims.Select(c => c.ToSecurityClaim()));
        }
    }
}