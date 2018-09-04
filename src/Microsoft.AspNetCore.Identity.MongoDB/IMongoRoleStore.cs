using System;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public interface IMongoRoleStore<TRole> : 
        IRoleStore<TRole>, 
        IQueryableRoleStore<TRole>,
        IRoleValidator<TRole>,
        IRoleClaimStore<TRole>
        where TRole : IdentityRole
    {
    }
}
