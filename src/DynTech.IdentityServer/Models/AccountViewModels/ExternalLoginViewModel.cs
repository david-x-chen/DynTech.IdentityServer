using System.ComponentModel.DataAnnotations;

namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// External login view model.
    /// </summary>
    public class ExternalLoginViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
