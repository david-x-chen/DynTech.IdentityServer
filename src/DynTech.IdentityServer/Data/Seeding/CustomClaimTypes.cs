namespace DynTech.IdentityServer.Data
{
    /// <summary>
    /// Custom claim types.
    /// </summary>
    public static class CustomClaimTypes
    {
        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public static string Permission
        {
            get
            {
                return "http://dyntech.solutions/claims/permission";
            }
        }
    }
}