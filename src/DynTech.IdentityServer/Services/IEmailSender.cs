using System.Threading.Tasks;

namespace DynTech.IdentityServer.Services
{
    /// <summary>
    /// Email sender.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the email async.
        /// </summary>
        /// <returns>The email async.</returns>
        /// <param name="email">Email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
