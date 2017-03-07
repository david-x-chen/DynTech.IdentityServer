using System.ComponentModel.DataAnnotations;

namespace Vanda.IdentityServer.Models.AccountViewModels
{
    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; }
        [Required]        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
