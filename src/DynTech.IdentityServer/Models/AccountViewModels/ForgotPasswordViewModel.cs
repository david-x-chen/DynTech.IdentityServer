using System.ComponentModel.DataAnnotations;

namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// Forgot password view model.
    /// </summary>
    public class ForgotPasswordViewModel
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
