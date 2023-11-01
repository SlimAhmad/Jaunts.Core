using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Jaunts.Core.Api.Brokers.RoleManagement
{
    public class RoleManagementBroker : IRoleManagementBroker
    {
        private readonly RoleManager<ApplicationRole> roleManagement;
        private readonly UserManager<ApplicationUser> userManagement;

        public RoleManagementBroker(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManagement = roleManager;

        }

        public RoleManagementBroker(RoleManager<ApplicationRole> roleManager,
                                   UserManager<ApplicationUser> userManager)
        {
            this.roleManagement = roleManager;
            this.userManagement = userManager;
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

        public async ValueTask<IdentityResult> InsertUserRoleAsync(ApplicationUser user, string role)
        {
            var broker = new RoleManagementBroker(this.roleManagement, this.userManagement);
            return await broker.userManagement.AddToRoleAsync(user, role);
        }

        public async ValueTask<IdentityResult> DeleteUserRoleAsync(ApplicationUser user, string role)
        {
            var broker = new RoleManagementBroker(this.roleManagement, this.userManagement);
            return await broker.userManagement.RemoveFromRoleAsync(user,role); 
        }

        public async ValueTask<IQueryable<ApplicationUser>> SelectUserRoles(ApplicationUser user) 
        {
            var broker = new RoleManagementBroker(this.roleManagement, this.userManagement);
            return (IQueryable<ApplicationUser>)await broker.userManagement.GetRolesAsync(user);
        }

        public async ValueTask<bool> SelectRoleExist(string role)
        {
            var broker = new RoleManagementBroker(this.roleManagement);
            return await broker.roleManagement.RoleExistsAsync(role);
        }
    }
}
