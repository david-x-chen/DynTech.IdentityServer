using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DynTech.IdentityServer.Services;

namespace DynTech.IdentityServer.Extensions
{
    /// <summary>
    /// Email sender extensions.
    /// </summary>
    public static class EmailSenderExtensions
    {
        /// <summary>
        /// Sends the email confirmation async.
        /// </summary>
        /// <returns>The email confirmation async.</returns>
        /// <param name="emailSender">Email sender.</param>
        /// <param name="email">Email.</param>
        /// <param name="link">Link.</param>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}