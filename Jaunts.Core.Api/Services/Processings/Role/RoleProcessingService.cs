using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Role;
using Jaunts.Core.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Web;

namespace Jaunts.Core.Api.Services.Processings.Role
{
    public partial class RoleProcessingService : IRoleProcessingService
    {
      
            private readonly IRoleService  roleService;
            private readonly ILoggingBroker loggingBroker;

            public RoleProcessingService(
                IRoleService  roleService,
                ILoggingBroker loggingBroker

                )
            {
                this.roleService = roleService;
                this.loggingBroker = loggingBroker;

            }

           public IQueryable<ApplicationRole> RetrieveAllRoles() =>
           TryCatch(() => this.roleService.RetrieveAllRoles());
           public ValueTask<bool> RemoveRoleByIdAsync(
                Guid id) =>
            TryCatch(async () =>
            {
                ValidateRoleId(id);
                var userRole = await roleService.RemoveRoleByIdRequestAsync(id);
                ValidateRole(userRole);
                return true;
            });

            public ValueTask<ApplicationRole> RetrieveRoleByIdAsync(
                Guid id) =>
            TryCatch(async () =>
            {
                ValidateRoleId(id);
                var role = await roleService.RetrieveRoleByIdRequestAsync(id);
                ValidateRole(role);
                return role ;
            });

            public ValueTask<ApplicationRole> UpsertRoleAsync(
                ApplicationRole  role) =>
            TryCatch(async () =>
            {
                ValidateRole(role);
                ApplicationRole maybeRole = RetrieveMatchingRole(role);

                return maybeRole switch
                {
                    null => await this.roleService.AddRoleRequestAsync(role),
                    _ => await this.roleService.ModifyRoleRequestAsync(role)
                };
            });


            public ValueTask<int> RetrievePermissions(List<string> role) =>
            TryCatch(async () =>
            {
                ValidateRoleList(role);
                var userRoles =  roleService.RetrieveAllRoles()
                              .Where(r => role.Contains(r.Name!)).ToList();

                var userPermissions = Permissions.None;

                foreach (var rolePermission in userRoles)
                    userPermissions |= rolePermission.Permissions;

                var permissionsValue = (int)userPermissions;
                return permissionsValue;

            });


        private ApplicationRole RetrieveMatchingRole(ApplicationRole role)
        {
            IQueryable<ApplicationRole> roles =
                this.roleService.RetrieveAllRoles();

            return roles.FirstOrDefault(SameApplicationRoleAs(role));
        }

        private static Expression<Func<ApplicationRole, bool>> SameApplicationRoleAs(ApplicationRole role) =>
            retrieveApplicationRole => retrieveApplicationRole.Id == role.Id;





    }
}
