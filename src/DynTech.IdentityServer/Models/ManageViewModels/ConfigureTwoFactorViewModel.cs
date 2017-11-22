using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynTech.IdentityServer.Models.ManageViewModels
{
    /// <summary>
    /// Configure two factor view model.
    /// </summary>
    public class ConfigureTwoFactorViewModel
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
    }
}
