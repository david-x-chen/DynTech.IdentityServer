using System.Collections.Generic;
using System.Linq;

namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel : LoginInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LoginViewModel"/> allow remember login.
        /// </summary>
        /// <value><c>true</c> if allow remember login; otherwise, <c>false</c>.</value>
        public bool AllowRememberLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LoginViewModel"/> enable local login.
        /// </summary>
        /// <value><c>true</c> if enable local login; otherwise, <c>false</c>.</value>
        public bool EnableLocalLogin { get; set; }

        /// <summary>
        /// Gets or sets the external providers.
        /// </summary>
        /// <value>The external providers.</value>
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }

        /// <summary>
        /// Gets the visible external providers.
        /// </summary>
        /// <value>The visible external providers.</value>
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LoginViewModel"/> is external login only.
        /// </summary>
        /// <value><c>true</c> if is external login only; otherwise, <c>false</c>.</value>
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

        /// <summary>
        /// Gets the external login scheme.
        /// </summary>
        /// <value>The external login scheme.</value>
        public string ExternalLoginScheme => ExternalProviders?.SingleOrDefault()?.AuthenticationScheme;

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.LoginViewModel"/> remember me.
        /// </summary>
        /// <value><c>true</c> if remember me; otherwise, <c>false</c>.</value>
        public bool RememberMe { get; internal set; }
    }

    /// <summary>
    /// External provider.
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the authentication scheme.
        /// </summary>
        /// <value>The authentication scheme.</value>
        public string AuthenticationScheme { get; set; }
    }
}
