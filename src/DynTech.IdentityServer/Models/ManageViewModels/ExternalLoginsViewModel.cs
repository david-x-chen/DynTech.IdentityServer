using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DynTech.IdentityServer.Models.ManageViewModels
{
    /// <summary>
    /// External logins view model.
    /// </summary>
    public class ExternalLoginsViewModel
    {
        /// <summary>
        /// Gets or sets the current logins.
        /// </summary>
        /// <value>The current logins.</value>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets the other logins.
        /// </summary>
        /// <value>The other logins.</value>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.ManageViewModels.ExternalLoginsViewModel"/> show remove button.
        /// </summary>
        /// <value><c>true</c> if show remove button; otherwise, <c>false</c>.</value>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage { get; set; }
    }
}