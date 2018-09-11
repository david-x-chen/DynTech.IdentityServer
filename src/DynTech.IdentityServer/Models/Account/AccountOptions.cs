namespace DynTech.IdentityServer.Models.Account
{
    /// <summary>
    /// Account options.
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// The show logout prompt.
        /// </summary>
        public static bool ShowLogoutPrompt = false;

        /// <summary>
        /// The automatic redirect after sign out.
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut = true;
    }
}