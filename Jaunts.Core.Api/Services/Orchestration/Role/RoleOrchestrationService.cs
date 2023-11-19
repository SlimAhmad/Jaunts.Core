// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;

namespace Jaunts.Core.Api.Services.Orchestration.Role
{
    public partial class RoleOrchestrationService : IRoleOrchestrationService
    {
        private readonly IRoleProcessingService roleProcessingService;
        private readonly IUserProcessingService userProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public RoleOrchestrationService(
            IRoleProcessingService roleProcessingService,
            IUserProcessingService userProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.roleProcessingService =  roleProcessingService ;
            this.userProcessingService = userProcessingService;
            this.loggingBroker = loggingBroker;

        }

        public ValueTask<bool> RemoveRoleByIdAsync(Guid id) =>
        TryCatch(async () => await this.roleProcessingService.RemoveRoleByIdAsync(id));
        public IQueryable<ApplicationRole> RetrieveAllRoles() =>
        TryCatch(() => this.roleProcessingService.RetrieveAllRoles());
        public ValueTask<int> RetrievePermissions(ApplicationUser user) =>
        TryCatch(async () => 
        {
            var userRoles = await userProcessingService.RetrieveUserRolesAsync(user);
            return await this.roleProcessingService.RetrievePermissions(userRoles);  
        });
        public ValueTask<ApplicationRole> RetrieveRoleById(Guid id) =>
        TryCatch(async () => await this.roleProcessingService.RetrieveRoleById(id));
        public ValueTask<ApplicationRole> UpsertRoleAsync(ApplicationRole role) =>
        TryCatch(async () => await this.roleProcessingService.UpsertRoleAsync(role));
    }
}
