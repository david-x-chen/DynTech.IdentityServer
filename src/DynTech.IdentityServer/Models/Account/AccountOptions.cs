namespace DynTech.IdentityServer.Models.Account
{
    /// <summary>
    /// Account options.
    /// </summary>
    public static class AccountOptions
    {
        /// <summary>
        /// The show logout prompt.
        /// </summary>
        public static bool ShowLogoutPrompt { get; private set; } = false;

        /// <summary>
        /// The automatic redirect after sign out.
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut { get; private set; } = true;
    }
}