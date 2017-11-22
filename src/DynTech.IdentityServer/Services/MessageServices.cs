using System.Threading.Tasks;

namespace DynTech.IdentityServer.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    /// <summary>
    /// Auth message sender.
    /// </summary>
    public class AuthMessageSender : IEmailSender
    {
        /// <summary>
        /// Sends the email async.
        /// </summary>
        /// <returns>The email async.</returns>
        /// <param name="email">Email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}
