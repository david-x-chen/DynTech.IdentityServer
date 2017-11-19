namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// Logout view model.
    /// </summary>
   public class LogoutViewModel : LogoutInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LogoutViewModel"/> show logout prompt.
        /// </summary>
        /// <value><c>true</c> if show logout prompt; otherwise, <c>false</c>.</value>
        public bool ShowLogoutPrompt { get; set; }
    }
}
