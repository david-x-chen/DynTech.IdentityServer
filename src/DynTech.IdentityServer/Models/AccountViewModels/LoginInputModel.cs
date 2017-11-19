using System.ComponentModel.DataAnnotations;

namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// Login input model.
    /// </summary>
    public class LoginInputModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LoginInputModel"/> remember login.
        /// </summary>
        /// <value><c>true</c> if remember login; otherwise, <c>false</c>.</value>
        public bool RememberLogin { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public string ReturnUrl { get; set; }
    }
}
