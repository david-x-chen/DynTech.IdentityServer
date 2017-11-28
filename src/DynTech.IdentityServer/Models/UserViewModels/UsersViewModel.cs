using System.Collections.Generic;

namespace DynTech.IdentityServer.Models.UserViewModels
{
    /// <summary>
    /// Users view model.
    /// </summary>
    public class UsersViewModel
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public IEnumerable<UserViewModel> Users { get; set; } 
    }

    /// <summary>
    /// User view model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the client names.
        /// </summary>
        /// <value>The client names.</value>
        public List<string> ClientNames { get; set; }
    }
}
