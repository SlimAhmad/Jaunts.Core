using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Brokers.RoleManagement
{
    public interface IRoleManagementBroker
    {
        ValueTask<ApplicationRole> InsertRoleAsync(ApplicationRole user);
        IQueryable<ApplicationRole> SelectAllRoles();
        ValueTask<ApplicationRole> SelectRoleByIdAsync(Guid userId);
        ValueTask<ApplicationRole> UpdateRoleAsync(ApplicationRole user);
        ValueTask<ApplicationRole> DeleteRoleAsync(ApplicationRole user);
    }
}
