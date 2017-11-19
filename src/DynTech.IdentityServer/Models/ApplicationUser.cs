using Microsoft.AspNetCore.Identity.MongoDB;

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
    }
}
