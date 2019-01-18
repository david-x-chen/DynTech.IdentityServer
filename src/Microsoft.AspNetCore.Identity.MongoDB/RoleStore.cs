
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
// I'm using async methods to leverage implicit Task wrapping of results from expression bodied functions.

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using global::MongoDB.Driver;

    /// <summary>
    ///     Note: Deleting and updating do not modify the roles stored on a user document. If you desire this dynamic
    ///     capability, override the appropriate operations on RoleStore as desired for your application. For example you could
    ///     perform a document modification on the users collection before a delete or a rename.
    ///     When passing a cancellation token, it will only be used if the operation requires a database interaction.
    /// </summary>
    /// <typeparam name="TRole">Needs to extend the provided IdentityRole type.</typeparam>
    public sealed class RoleStore<TRole> : IMongoRoleStore<TRole> where TRole : IdentityRole
    {
		private readonly IMongoCollection<TRole> _Roles;

		public RoleStore(IMongoCollection<TRole> roles)
		{
			_Roles = roles;
		}

		public void Dispose()
		{
            // no need to dispose of anything, mongodb handles connection pooling automatically
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _Roles.InsertOneAsync(role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			var result = await _Roles.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
            if (result.IsAcknowledged)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError());
            }
        }

		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			var result = await _Roles.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
			if (result.IsAcknowledged)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError());
            }
		}

		public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
			=> role.Id;

		public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
			=> role.Name;

		public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
			=> role.Name = roleName;

		// note: can't test as of yet through integration testing because the Identity framework doesn't use this method internally anywhere
		public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
			=> role.NormalizedName;

		public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
			=> role.NormalizedName = normalizedName;

		public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
			=> _Roles.Find(r => r.Id == roleId)
				.FirstOrDefaultAsync(cancellationToken);

        public Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken)
			=> _Roles.Find(r => r.NormalizedName == normalizedName)
				.FirstOrDefaultAsync(cancellationToken);

        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken)
            => role.Claims.Select(c => c.ToSecurityClaim()).ToList();

        public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken)
        {
            role.AddClaim(claim);

            await UpdateAsync(role, cancellationToken);
        }

        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken)
        {
            role.RemoveClaim(claim);

            await UpdateAsync(role, cancellationToken);
        }

        public async Task<IdentityResult> ValidateAsync(RoleManager<TRole> manager, TRole role)
        {
            var exists = await manager.RoleExistsAsync(role.NormalizedName);

            if (result.IsAcknowledged)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError());
            }
        }

        public IQueryable<TRole> Roles
			=> _Roles.AsQueryable();
	}
}