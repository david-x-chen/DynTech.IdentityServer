using System;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public interface IMongoUserStore<TUser> :
            IUserPasswordStore<TUser>,
            IUserRoleStore<TUser>,
            IUserLoginStore<TUser>,
            IUserSecurityStampStore<TUser>,
            IUserEmailStore<TUser>,
            IUserClaimStore<TUser>,
            IUserPhoneNumberStore<TUser>,
            IUserTwoFactorStore<TUser>,
            IUserTwoFactorRecoveryCodeStore<TUser>,
            IUserLockoutStore<TUser>,
            IQueryableUserStore<TUser>,
            IUserAuthenticatorKeyStore<TUser>,
            IUserAuthenticationTokenStore<TUser>
        where TUser : IdentityUser
    {
    }
}
