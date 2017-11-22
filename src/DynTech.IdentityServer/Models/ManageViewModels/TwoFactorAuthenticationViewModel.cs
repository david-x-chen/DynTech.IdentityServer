namespace DynTech.IdentityServer.Models.ManageViewModels
{
    /// <summary>
    /// Two factor authentication view model.
    /// </summary>
    public class TwoFactorAuthenticationViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.ManageViewModels.TwoFactorAuthenticationViewModel"/> has authenticator.
        /// </summary>
        /// <value><c>true</c> if has authenticator; otherwise, <c>false</c>.</value>
        public bool HasAuthenticator { get; set; }

        /// <summary>
        /// Gets or sets the recovery codes left.
        /// </summary>
        /// <value>The recovery codes left.</value>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.ManageViewModels.TwoFactorAuthenticationViewModel"/> is2fa enabled.
        /// </summary>
        /// <value><c>true</c> if is2fa enabled; otherwise, <c>false</c>.</value>
        public bool Is2faEnabled { get; set; }
    }
}