using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Role
{
    public partial interface IRoleProcessingService
    {
        IQueryable<ApplicationRole> RetrieveAllRoles();
        ValueTask<bool> RemoveRoleByIdAsync(Guid id);
        ValueTask<ApplicationRole> RetrieveRoleByIdAsync(Guid id);
        ValueTask<ApplicationRole> UpsertRoleAsync(ApplicationRole role);
        ValueTask<int> RetrievePermissions(List<string> role);

    }
}
