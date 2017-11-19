using System.Linq;
using DynTech.IdentityServer.Configuration;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;

namespace DynTech.IdentityServer.Data.Seeding
{
    /// <summary>
    /// Seed mongo DBD ata.
    /// </summary>
    public static class SeedMongoDBData
    {
        /// <summary>
        /// Ensures the seed data.
        /// </summary>
        /// <param name="context">Context.</param>
        public static void EnsureSeedData(IConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Clients.Get().ToList())
                {
                    context.AddClient(client.ToEntity());
                }
            }

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
    }
}