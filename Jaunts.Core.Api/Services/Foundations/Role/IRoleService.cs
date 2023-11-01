using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Services.Foundations.Role
{
    public partial interface IRoleService
    {
        ValueTask<ApplicationRole> RegisterRoleRequestAsync(ApplicationRole role);
        IQueryable<ApplicationRole> RetrieveAllRoles();
        ValueTask<ApplicationRole> RetrieveRoleByIdRequestAsync(Guid roleId);
        ValueTask<ApplicationRole> ModifyRoleRequestAsync(ApplicationRole role);
        ValueTask<ApplicationRole> RemoveRoleByIdRequestAsync(Guid roleId);
    }
}
