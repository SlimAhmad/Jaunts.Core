using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.Role
{
    public partial interface IRoleOrchestrationService
    {
        IQueryable<ApplicationRole> RetrieveAllRoles();
        ValueTask<bool> RemoveRoleByIdAsync(Guid id);
        ValueTask<ApplicationRole> RetrieveRoleById(Guid id);
        ValueTask<ApplicationRole> UpsertRoleAsync(ApplicationRole role);
        ValueTask<int> RetrievePermissions(ApplicationUser user);
    }
}
