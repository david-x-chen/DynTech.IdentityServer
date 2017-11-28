using IdentityServer4.Models;
using System.Collections.Generic;

namespace DynTech.IdentityServer.Configuration
{
    /// <summary>
    /// </summary>
    public class Resources
    {
        /// <summary>
        /// </summary>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),

                // custom identity resource with some consolidated claims
                new IdentityResource("apiscope",new []{ "role", "admin", "user", "api", "api.admin" , "api.user" } )
            };
        }

        /// <summary>
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api")
                {
                    ApiSecrets =
                    {
                        new Secret("apiSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "apiscope",
                            DisplayName = "Scope for the api service"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "api", "api.admin", "api.user" }
                }
            };
        }
    }
}
