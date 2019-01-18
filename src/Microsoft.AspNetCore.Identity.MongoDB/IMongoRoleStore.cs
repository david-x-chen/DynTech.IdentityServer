using System;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public interface IMongoRoleStore<TRole> : 
        IQueryableRoleStore<TRole>,
        IRoleValidator<TRole>,
        IRoleClaimStore<TRole>
        where TRole : IdentityRole
    {
    }
}
