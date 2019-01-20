using IdentityServer4.Models;
using System.Collections.Generic;

namespace DynTech.IdentityServer.Configuration
{
    /// <summary>
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("custom-api")
                {
                    ApiSecrets =
                    {
                        new Secret("apiSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "custom-scope",
                            DisplayName = "Scope for the api service"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "api", "api.admin", "api.user" }
                }
            };
        }
    }
}
