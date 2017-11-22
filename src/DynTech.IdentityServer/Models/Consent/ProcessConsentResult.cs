namespace DynTech.IdentityServer.Models.Consent
{
    /// <summary>
    /// Process consent result.
    /// </summary>
    public class ProcessConsentResult
    {
        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.Consent.ProcessConsentResult"/> is redirect.
        /// </summary>
        /// <value><c>true</c> if is redirect; otherwise, <c>false</c>.</value>
        public bool IsRedirect => RedirectUri != null;

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>The redirect URI.</value>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.Consent.ProcessConsentResult"/> show view.
        /// </summary>
        /// <value><c>true</c> if show view; otherwise, <c>false</c>.</value>
        public bool ShowView => ViewModel != null;

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public ConsentViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.Consent.ProcessConsentResult"/> has validation error.
        /// </summary>
        /// <value><c>true</c> if has validation error; otherwise, <c>false</c>.</value>
        public bool HasValidationError => ValidationError != null;

        /// <summary>
        /// Gets or sets the validation error.
        /// </summary>
        /// <value>The validation error.</value>
        public string ValidationError { get; set; }
    }
}