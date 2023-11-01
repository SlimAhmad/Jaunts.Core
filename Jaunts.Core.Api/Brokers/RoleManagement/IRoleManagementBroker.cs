using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.RoleManagement
{
    public interface IRoleManagementBroker
    {
        ValueTask<ApplicationRole> InsertRoleAsync(ApplicationRole role);
        ValueTask<IdentityResult> InsertUserRoleAsync(ApplicationUser user,string role);
        ValueTask<IdentityResult> DeleteUserRoleAsync(ApplicationUser user, string role);
        IQueryable<ApplicationRole> SelectAllRoles();
        ValueTask<bool> SelectRoleExist(string role);
        ValueTask<IQueryable<ApplicationUser>> SelectUserRoles(ApplicationUser user);
        ValueTask<ApplicationRole> SelectRoleByIdAsync(Guid roleId);
        ValueTask<ApplicationRole> UpdateRoleAsync(ApplicationRole role);
        ValueTask<ApplicationRole> DeleteRoleAsync(ApplicationRole role);
    }
}
