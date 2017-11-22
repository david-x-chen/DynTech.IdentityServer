using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynTech.IdentityServer.Models.AccountViewModels
{
    /// <summary>
    /// Send code view model.
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Gets or sets the selected provider.
        /// </summary>
        /// <value>The selected provider.</value>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public ICollection<SelectListItem> Providers { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:DynTech.IdentityServer.Models.AccountViewModels.SendCodeViewModel"/> remember me.
        /// </summary>
        /// <value><c>true</c> if remember me; otherwise, <c>false</c>.</value>
        public bool RememberMe { get; set; }
    }
}
