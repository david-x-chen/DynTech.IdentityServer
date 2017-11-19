using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using DynTech.IdentityServer.Models;

namespace DynTech.IdentityServer.Services
{
    /// <summary>
    /// User claims profile service.
    /// </summary>
    public class UserClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynTech.IdentityServer.Services.UserClaimsProfileService"/> class.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="claimsFactory">Claims factory.</param>
        public UserClaimsProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        /// <summary>
        /// Gets the profile data async.
        /// </summary>
        /// <returns>The profile data async.</returns>
        /// <param name="context">Context.</param>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

            if (user.IsAdmin)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
            }

            // Update client
            if (user.ApiRoles == "api.admin")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "api.admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "api.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "api"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "api"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "api.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "api"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "api"));
            }

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

            context.IssuedClaims = claims;
        }

        /// <summary>
        /// Ises the active async.
        /// </summary>
        /// <returns>The active async.</returns>
        /// <param name="context">Context.</param>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}