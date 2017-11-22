using System.ComponentModel.DataAnnotations;

namespace DynTech.IdentityServer.Models.ManageViewModels
{
    /// <summary>
    /// Add phone number view model.
    /// </summary>
    public class AddPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
