using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace DynTech.IdentityServer
{
    /// <summary>
    /// Extension grant validator.
    /// </summary>
    public class ExtensionGrantValidator : IExtensionGrantValidator
    {
        /// <summary>
        /// Validates the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="context">Context.</param>
        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var credential = context.Request.Raw.Get("custom_credential");

            if (credential != null)
            {
                context.Result = new GrantValidationResult(subject: "818727", authenticationMethod: "custom");
            }
            else
            {
                // custom error message
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the type of the grant.
        /// </summary>
        /// <value>The type of the grant.</value>
        public string GrantType
        {
            get { return "custom"; }
        }
    }
}