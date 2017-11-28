using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace DynTech.IdentityServer.Configuration
{
    /// <summary>
    /// </summary>
    public class Clients
    {
        /// <summary>
        /// </summary>
        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> Get()
        {
            var updateClientUrl = "https://update.fake.com";
            var smartCMSUrl = "https://smartcms.fake.com";

            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "DynTechUpdate",
                    ClientName = "DynTech Update Service Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        updateClientUrl + "/authorized"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        updateClientUrl + "/unauthorized.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        updateClientUrl,
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "email",
                        "profile",
                        "updateApi",
                        "updateapiscope",
                    }
                },
                new Client
                {
                    ClientId = "DynTechCHAS",
                    ClientName = "DynTech CHAS Service Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        smartCMSUrl
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        smartCMSUrl + "/Unauthorized"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        smartCMSUrl,
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "email",
                        "profile",
                        "chasApi",
                        "chasapiscope",
                    }
                }
            };
        }
    }
}
