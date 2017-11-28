using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynTech.IdentityServer.Models.ClientViewModels
{
    /// <summary>
    /// Clients view model.
    /// </summary>
    public class ClientsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DynTech.IdentityServer.Models.ClientViewModels.ClientsViewModel"/> class.
        /// </summary>
        public ClientsViewModel()
        {
            Clients = new List<ClientViewModel>();
        }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>The clients.</value>
        public List<ClientViewModel> Clients { get; set; }
    }

    /// <summary>
    /// Client view model.
    /// </summary>
    public class ClientViewModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DynTech.IdentityServer.Models.ClientViewModels.ClientViewModel"/> class.
        /// </summary>
        public ClientViewModel()
        {
            RedirectUris = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            AllowedCorsOrigins = new List<string>();
            AllowedScopes = DefaultScopes;
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        [Required]
        [Display(Name = "Client Id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>The client secret.</value>
        [Required]
        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect uris.
        /// </summary>
        /// <value>The redirect uris.</value>
        public List<string> RedirectUris { get; set; }

        /// <summary>
        /// Gets the redirect uris string.
        /// </summary>
        /// <value>The redirect uris string.</value>
        [MinLength(5)]
        [MaxLength(1024)]
        [Display(Name = "Redirect Uris")]
        public string RedirectUrisString { get; set; }

        /// <summary>
        /// Gets or sets the post logout redirect uris.
        /// </summary>
        /// <value>The post logout redirect uris.</value>
        public List<string> PostLogoutRedirectUris { get; set; }

        /// <summary>
        /// Gets the post logout redirect uris string.
        /// </summary>
        /// <value>The post logout redirect uris string.</value>
        [MinLength(5)]
        [MaxLength(1024)]
        [Display(Name = "Post Logout Redirect Uris")]
        public string PostLogoutRedirectUrisString { get; set; }

        /// <summary>
        /// Gets or sets the allowed cors origins.
        /// </summary>
        /// <value>The allowed cors origins.</value>
        public List<string> AllowedCorsOrigins { get; set; }

        /// <summary>
        /// Gets the allowed cors origins string.
        /// </summary>
        /// <value>The allowed cors origins string.</value>
        [MinLength(5)]
        [MaxLength(1024)]
        [Display(Name = "Allowed Cors Origins")]
        public string AllowedCorsOriginsString { get; set; }

        /// <summary>
        /// The allowed scopes.
        /// </summary>
        public List<string> AllowedScopes { get; set; }

        /// <summary>
        /// The default scopes.
        /// </summary>
        public List<string> DefaultScopes = new List<string>
        {
            "openid",
            "email",
            "profile"
        };
    }
}
