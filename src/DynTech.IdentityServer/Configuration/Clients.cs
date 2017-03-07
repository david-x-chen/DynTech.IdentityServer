using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Vanda.IdentityServer.Configuration
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
            var updateClientUrl = "https://update.magicsoft-asia.com";
            var smartCMSUrl = "https://smartcms.medicare-asia.com";

            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "vandaUpdate",
                    ClientName = "Vanda Update Service Client",
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
                    ClientId = "vandaCHAS",
                    ClientName = "Vanda CHAS Service Client",
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
