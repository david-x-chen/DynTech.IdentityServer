namespace DynTech.IdentityServer.Models.Consent
{
    /// <summary>
    /// Consent options.
    /// </summary>
    public class ConsentOptions
    {
        /// <summary>
        /// The enable offline access.
        /// </summary>
        public static bool EnableOfflineAccess { get; private set; } = true;

        /// <summary>
        /// The name of the offline access display.
        /// </summary>
        public static string OfflineAccessDisplayName { get; private set; } = "Offline Access";

        /// <summary>
        /// The offline access description.
        /// </summary>
        public static string OfflineAccessDescription { get; private set; } = "Access to your applications and resources, even when you are offline";

        /// <summary>
        /// The much choose one error message.
        /// </summary>
        public static readonly string MuchChooseOneErrorMessage = "You must pick at least one permission";

        /// <summary>
        /// The invalid selection error message.
        /// </summary>
        public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
    }
}