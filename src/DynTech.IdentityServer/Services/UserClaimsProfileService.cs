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
    public class UserClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserClaimsProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

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
            if (user.UpdateApiRoles == "updateApi.admin")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "updateApi.admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "updateApi.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "updateApi"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "updateApi"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "updateApi.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "updateApi"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "updateApi"));
            }

            if (user.ChasApiRoles == "chasApi.admin")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "chasApi.admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "chasApi.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "chasApi"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "chasApi"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "chasApi.user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "chasApi"));
                claims.Add(new Claim(JwtClaimTypes.Scope, "chasApi"));
            }

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}