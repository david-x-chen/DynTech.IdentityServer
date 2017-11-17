using Microsoft.AspNetCore.Identity;

namespace Vanda.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {        
        public bool IsAdmin { get; set; }
        public string UpdateApiRoles { get; set; }
        public string ChasApiRoles { get; set; }
    }
}
