using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Role;

namespace Jaunts.Core.Api.Services.Foundations.Role
{
    public partial class RoleService : IRoleService
    {

        private readonly IRoleManagementBroker roleManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RoleService(
            IRoleManagementBroker roleManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.roleManagementBroker = roleManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ApplicationRole> RegisterRoleRequestAsync(ApplicationRole role) =>
         TryCatch(async () =>
         {
             ValidateRoleOnCreate(role);
             return await this.roleManagementBroker.InsertRoleAsync(role);
         });

        public IQueryable<ApplicationRole> RetrieveAllRoles() =>
        TryCatch(() => this.roleManagementBroker.SelectAllRoles());

        public ValueTask<ApplicationRole> RetrieveRoleByIdRequestAsync(Guid roleId) =>
        TryCatch(async () =>
        {
            ValidateRoleId(roleId);
            ApplicationRole storageRole = await this.roleManagementBroker.SelectRoleByIdAsync(roleId);
            ValidateStorageRole(storageRole, roleId);

            return storageRole;
        });

        public ValueTask<ApplicationRole> ModifyRoleRequestAsync(ApplicationRole role) =>
        TryCatch(async () =>
        {
            ValidateRoleOnModify(role);
            ApplicationRole maybeRole = await this.roleManagementBroker.SelectRoleByIdAsync(role.Id);
            ValidateStorageRole(maybeRole, role.Id);
            ValidateAgainstStorageRoleOnModify(inputRole: role, storageRole: maybeRole);

            return await this.roleManagementBroker.UpdateRoleAsync(role);
        });

        public ValueTask<ApplicationRole> RemoveRoleByIdRequestAsync(Guid roleId) =>
        TryCatch(async () =>
        {
            ValidateRoleId(roleId);
            ApplicationRole maybeRole = await this.roleManagementBroker.SelectRoleByIdAsync(roleId);
            ValidateStorageRole(maybeRole, roleId);

            return await this.roleManagementBroker.DeleteRoleAsync(maybeRole);
        });
    }
}
