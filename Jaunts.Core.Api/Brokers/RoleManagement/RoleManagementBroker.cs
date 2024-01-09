using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.RoleManagement
{
    public class RoleManagementBroker : IRoleManagementBroker
    {
        private readonly RoleManager<ApplicationRole> roleManagement;

        public RoleManagementBroker(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManagement = roleManager;

        }
        public IQueryable<ApplicationRole> SelectAllRoles() => this.roleManagement.Roles;

        public async ValueTask<ApplicationRole> SelectRoleByIdAsync(Guid roleId)
        {
            var broker = new RoleManagementBroker(this.roleManagement);

            return await broker.roleManagement.FindByIdAsync(roleId.ToString());
        }

        public async ValueTask<ApplicationRole> InsertRoleAsync(ApplicationRole role)
        {
            var broker = new RoleManagementBroker(this.roleManagement);
            await broker.roleManagement.CreateAsync(role);

            return role;
        }

        public async ValueTask<ApplicationRole> UpdateRoleAsync(ApplicationRole role)
        {
            var broker = new RoleManagementBroker(this.roleManagement);
            await broker.roleManagement.UpdateAsync(role);

            return role;
        }

        public async ValueTask<ApplicationRole> DeleteRoleAsync(ApplicationRole role)
        {
            var broker = new RoleManagementBroker(this.roleManagement);
            await broker.roleManagement.DeleteAsync(role);

            return role;
        }
    }
}
