using DynTech.IdentityServer.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DynTech.IdentityServer.Extensions
{
    /// <summary>
    /// URL helper extensions.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Emails the confirmation link.
        /// </summary>
        /// <returns>The confirmation link.</returns>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="code">Code.</param>
        /// <param name="scheme">Scheme.</param>
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        /// <summary>
        /// Resets the password callback link.
        /// </summary>
        /// <returns>The password callback link.</returns>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="code">Code.</param>
        /// <param name="scheme">Scheme.</param>
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}