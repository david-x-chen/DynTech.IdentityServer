
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
// I'm using async methods to leverage implicit Task wrapping of results from expression bodied functions.

namespace Microsoft.AspNetCore.Identity.MongoDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading;
	using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    using global::MongoDB.Bson;
	using global::MongoDB.Driver;

	/// <summary>
	///     When passing a cancellation cancellationToken, it will only be used if the operation requires a database interaction.
	/// </summary>
	/// <typeparam name="TUser"></typeparam>
    public sealed class UserStore<TUser> : IMongoUserStore<TUser>
        where TUser : IdentityUser
    {
		private readonly IMongoCollection<TUser> _Users;
		private const string InternalLoginProvider = "[AspNetUserStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
		private const string RecoveryCodeTokenName = "RecoveryCodes";

		public UserStore(IMongoCollection<TUser> users)
		{
			_Users = users;
		}

		private bool _disposed = false;

        /// <summary>
        ///     Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
            var hasher = new PasswordHasher<TUser>();
            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

            await _Users.InsertOneAsync(user, cancellationToken: cancellationToken);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			// todo should add an optimistic concurrency check
			await _Users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
			// todo success based on replace result
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			await _Users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
			// todo success based on delete result
			return IdentityResult.Success;
		}

		public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
			=> user.Id;

		public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
			=> user.UserName;

		public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
			=> user.UserName = userName;

		// note: again this isn't used by Identity framework so no way to integration test it
		public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
			=> user.NormalizedUserName;

		public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
			=> user.NormalizedUserName = normalizedName;

		public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
			=> IsObjectId(userId)
				? _Users.Find(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken)
				: Task.FromResult<TUser>(null);

		private bool IsObjectId(string id)
		{
			ObjectId temp;
			return ObjectId.TryParse(id, out temp);
		}

		public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
			// todo low priority exception on duplicates? or better to enforce unique index to ensure this
			=> _Users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);

		public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
			=> user.PasswordHash = passwordHash;

		public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
			=> user.PasswordHash;

		public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            var builder = new FilterDefinitionBuilder<TUser>();
            var filter = builder.Eq(u => u.NormalizedEmail, user.NormalizedEmail);
            var userInfo = await _Users.FindAsync(filter);
            var existingUser = userInfo.FirstOrDefault();

            return existingUser != null && !string.IsNullOrEmpty(existingUser.PasswordHash);
        }

		public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
			=> user.AddRole(roleName);

		public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
			=> user.RemoveRole(roleName);

		// todo might have issue, I'm just storing Normalized only now, so I'm returning normalized here instead of not normalized.
		// EF provider returns not noramlized here
		// however, the rest of the API uses normalized (add/remove/isinrole) so maybe this approach is better anyways
		// note: could always map normalized to not if people complain
		public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
			=> user.Roles;

		public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
			=> user.Roles.Contains(roleName);

		public async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
			=> await _Users.Find(u => u.Roles.Contains(normalizedRoleName))
				.ToListAsync(cancellationToken);

		public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
			=> user.AddLogin(login);

		public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
			=> user.RemoveLogin(loginProvider, providerKey);

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
			=> user.Logins
				.Select(l => l.ToUserLoginInfo())
				.ToList();

		public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
			=> _Users
				.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey))
				.FirstOrDefaultAsync(cancellationToken);

		public async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
			=> user.SecurityStamp = stamp;

		public async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
			=> user.SecurityStamp;

		public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
			=> user.EmailConfirmed;

		public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
			=> user.EmailConfirmed = confirmed;

		public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
			=> user.Email = email;

		public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
			=> user.Email;

		// note: no way to intergation test as this isn't used by Identity framework	
		public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
			=> user.NormalizedEmail;

		public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
			=> user.NormalizedEmail = normalizedEmail;

		public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			// note: I don't like that this now searches on normalized email :(... why not FindByNormalizedEmailAsync then?
			// todo low - what if a user can have multiple accounts with the same email?
			return _Users.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
			=> user.Claims.Select(c => c.ToSecurityClaim()).ToList();

		public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			foreach (var claim in claims)
			{
				user.AddClaim(claim);
			}
			return Task.FromResult(0);
		}

		public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			foreach (var claim in claims)
			{
				user.RemoveClaim(claim);
			}
			return Task.FromResult(0);
		}

		public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
		{
			user.ReplaceClaim(claim, newClaim);
		}

		public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
		{
			user.PhoneNumber = phoneNumber;
			return Task.FromResult(0);
		}

		public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PhoneNumber);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PhoneNumberConfirmed);
		}

		public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			user.PhoneNumberConfirmed = confirmed;
			return Task.FromResult(0);
		}

		public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

		public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.TwoFactorEnabled);
		}

		public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			return await _Users
				.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
				.ToListAsync(cancellationToken);
		}

		public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
		{
			DateTimeOffset? dateTimeOffset = user.LockoutEndDateUtc;
			return Task.FromResult(dateTimeOffset);
		}

		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
		{
			user.LockoutEndDateUtc = lockoutEnd?.UtcDateTime;
			return Task.FromResult(0);
		}

		public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			user.AccessFailedCount++;
			return Task.FromResult(user.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			user.AccessFailedCount = 0;
			return Task.FromResult(0);
		}

		public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
			=> user.AccessFailedCount;

		public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
			=> user.LockoutEnabled;

		public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
			=> user.LockoutEnabled = enabled;

		public IQueryable<TUser> Users => _Users.AsQueryable();

		public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
			=> user.SetToken(loginProvider, name, value);

		public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
			=> user.RemoveToken(loginProvider, name);

		public async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
			=> user.GetTokenValue(loginProvider, name);

        public async Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
        {
            await SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
        }

        public async Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
        {
           return await GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
        }

        public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
			return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            return await RedeemCodeInternalAsync(user, code, cancellationToken);
        }

        private async Task<bool> RedeemCodeInternalAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
			var splitCodes = mergedCodes.Split(';');
			if (splitCodes.Contains(code))
			{
				var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
				await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
				return true;
			}
			return false;
        }

        public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await CountCodesInternalAsync(user, cancellationToken);
        }

        private async Task<int> CountCodesInternalAsync(TUser user, CancellationToken cancellationToken)
        { 
            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
			if (mergedCodes.Length > 0)
			{
				return mergedCodes.Split(';').Length;
			}
			return 0;
        }

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				//throw new ObjectDisposedException(GetType().Name);
			}
		}
    }
}