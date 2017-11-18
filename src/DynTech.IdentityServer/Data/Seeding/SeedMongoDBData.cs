using System.Linq;
using DynTech.IdentityServer.Configuration;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;

namespace DynTech.IdentityServer.Data.Seeding
{
    public static class SeedMongoDBData
    {
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