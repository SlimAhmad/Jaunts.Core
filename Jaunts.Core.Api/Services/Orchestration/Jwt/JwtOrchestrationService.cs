// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Jwt;
using Jaunts.Core.Api.Services.Processings.Jwt;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;

namespace Jaunts.Core.Api.Services.Orchestration.Jwt
{
    public partial class JwtOrchestrationService : IJwtOrchestrationService
    {
        private readonly IJwtProcessingService  jwtProcessingService;
        private readonly IUserProcessingService userProcessingService;
        private readonly IRoleProcessingService roleProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public JwtOrchestrationService(
            IJwtProcessingService  jwtProcessingService,
            IUserProcessingService userProcessingService,
            IRoleProcessingService rolesProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.jwtProcessingService = jwtProcessingService;
            this.userProcessingService = userProcessingService;
            this.roleProcessingService = rolesProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<UserAccountDetailsResponse> JwtAccountDetailsAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            var userRoles = await userProcessingService.RetrieveUserRolesAsync(user);
            int permissions = await this.roleProcessingService.RetrievePermissions(userRoles);
            string token = await this.jwtProcessingService.GenerateJwtTokenAsync(user, permissions);

            return ConvertToAccountDetailsResponse(user,token,userRoles);
        });


        private UserAccountDetailsResponse ConvertToAccountDetailsResponse(ApplicationUser user,string token,List<string> role)
        {

            return new UserAccountDetailsResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = token,
                Role = role,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
            };
        }

    }
}
