using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynTech.IdentityServer.Views.Manage
{
    /// <summary>
    /// Manage nav pages.
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Gets the active page key.
        /// </summary>
        /// <value>The active page key.</value>
        public static string ActivePageKey => "ActivePage";

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public static string Index => "Index";

        /// <summary>
        /// Gets the change password.
        /// </summary>
        /// <value>The change password.</value>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        /// Gets the external logins.
        /// </summary>
        /// <value>The external logins.</value>
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        /// Gets the two factor authentication.
        /// </summary>
        /// <value>The two factor authentication.</value>
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        /// Indexs the nav class.
        /// </summary>
        /// <returns>The nav class.</returns>
        /// <param name="viewContext">View context.</param>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Changes the password nav class.
        /// </summary>
        /// <returns>The password nav class.</returns>
        /// <param name="viewContext">View context.</param>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// Externals the logins nav class.
        /// </summary>
        /// <returns>The logins nav class.</returns>
        /// <param name="viewContext">View context.</param>
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        /// <summary>
        /// Twos the factor authentication nav class.
        /// </summary>
        /// <returns>The factor authentication nav class.</returns>
        /// <param name="viewContext">View context.</param>
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
    
        /// <summary>
        /// Pages the nav class.
        /// </summary>
        /// <returns>The nav class.</returns>
        /// <param name="viewContext">View context.</param>
        /// <param name="page">Page.</param>
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        /// <summary>
        /// Adds the active page.
        /// </summary>
        /// <param name="viewData">View data.</param>
        /// <param name="activePage">Active page.</param>
        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}