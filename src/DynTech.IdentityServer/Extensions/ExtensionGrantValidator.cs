using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace DynTech.IdentityServer
{
    public class ExtensionGrantValidator : IExtensionGrantValidator
    {
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

        public string GrantType
        {
            get { return "custom"; }
        }
    }
}