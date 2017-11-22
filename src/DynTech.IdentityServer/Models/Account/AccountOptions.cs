using System;

namespace DynTech.IdentityServer.Models.Account
{
    /// <summary>
    /// Account options.
    /// </summary>
    public class AccountOptions
    {
        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;
    }
}