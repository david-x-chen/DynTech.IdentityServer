using IdentityServer4.Models;
using System.Collections.Generic;

namespace Vanda.IdentityServer.Configuration
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

                // custom identity resource with some consolidated claims
                new IdentityResource("updateapiscope",new []{ "role", "admin", "user", "updateApi", "updateApi.admin" , "updateApi.user" } ),
                new IdentityResource("chasapiscope",new []{ "role", "admin", "user", "chasApi", "chasApi.admin", "chasApi.user"} )
            };
        }

        /// <summary>
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("updateApi")
                {
                    ApiSecrets =
                    {
                        new Secret("updateApiSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "updateapiscope",
                            DisplayName = "Scope for the update service"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "updateApi", "updateApi.admin", "updateApi.user" }
                },
                new ApiResource("chasApi")
                {
                    ApiSecrets =
                    {
                        new Secret("chasApiSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "chasapiscope",
                            DisplayName = "Scope for the CHAS service"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "chasApi", "chasApi.admin", "chasApi.user" }
                }
            };
        }
    }
}
