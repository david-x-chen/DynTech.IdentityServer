using System;

namespace DynTech.IdentityServer.Models.Account
{
    /// <summary>
    /// Account options.
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// The allow local login.
        /// </summary>
        public static bool AllowLocalLogin = true;

        /// <summary>
        /// The allow remember login.
        /// </summary>
        public static bool AllowRememberLogin = true;

        /// <summary>
        /// The duration of the remember me login.
        /// </summary>
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        /// <summary>
        /// The show logout prompt.
        /// </summary>
        public static bool ShowLogoutPrompt = true;

        /// <summary>
        /// The automatic redirect after sign out.
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut = false;

        // to enable windows authentication, the host (IIS or IIS Express) also must have 
        // windows auth enabled.
        /// <summary>
        /// The windows authentication enabled.
        /// </summary>
        public static bool WindowsAuthenticationEnabled = true;
        /// <summary>
        /// The include windows groups.
        /// </summary>
        public static bool IncludeWindowsGroups = false;

        // specify the Windows authentication scheme and display name
        /// <summary>
        /// The name of the windows authentication scheme.
        /// </summary>
        public static readonly string WindowsAuthenticationSchemeName = "Windows";

        /// <summary>
        /// The invalid credentials error message.
        /// </summary>
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}