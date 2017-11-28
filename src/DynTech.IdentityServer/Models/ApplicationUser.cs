using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;

namespace DynTech.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    /// <summary>
    /// Application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:DynTech.IdentityServer.Models.ApplicationUser"/>
        /// is admin.
        /// </summary>
        /// <value><c>true</c> if is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the update API roles.
        /// </summary>
        /// <value>The update API roles.</value>
        public string ApiRoles { get; set; }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>The clients.</value>
        public List<string> Clients { get; set; }

        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="clientId">Client identifier.</param>
        public virtual void AddClient(string clientId)
        {
            Clients.Add(clientId);
        }

        /// <summary>
        /// Removes the claim.
        /// </summary>
        /// <param name="clientId">Client identifier.</param>
        public virtual void RemoveClient(string clientId)
        {
            Clients.RemoveAll(c => c == clientId);
        }
    }
}
