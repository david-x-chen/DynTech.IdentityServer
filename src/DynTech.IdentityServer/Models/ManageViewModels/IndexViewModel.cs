using System.ComponentModel.DataAnnotations;

namespace DynTech.IdentityServer.Models.ManageViewModels
{
    /// <summary>
    /// Index view model.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.ManageViewModels.IndexViewModel"/> is email confirmed.
        /// </summary>
        /// <value><c>true</c> if is email confirmed; otherwise, <c>false</c>.</value>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage { get; set; }
    }
}
