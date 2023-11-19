using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.User
{
    public partial class UserOrchestrationService : IUserOrchestrationService
    {
        private readonly IUserProcessingService userProcessingService;
        private readonly ILoggingBroker loggingBroker;


        public UserOrchestrationService(
            IUserProcessingService userProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.userProcessingService = userProcessingService;
            this.loggingBroker = loggingBroker;

        }

        public ValueTask<ApplicationUser> CheckPasswordValidityAsync(string password, Guid id) =>
        TryCatch(async () => await this.userProcessingService.CheckPasswordValidityAsync(password,id));

        public ValueTask<ApplicationUser> ConfirmEmailAsync(string token, string email) =>
        TryCatch(async () => await this.userProcessingService.ConfirmEmailAsync(token,email));

        public ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.EmailConfirmationTokenAsync(user));

        public ValueTask<ApplicationUser> EnableOrDisable2FactorAuthenticationAsync(Guid id) =>
        TryCatch(async () => await this.userProcessingService.EnableOrDisable2FactorAuthenticationAsync(id));

        public ValueTask<bool> EnsureUserExistAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.EnsureUserExistAsync(user));

        public ValueTask<string> PasswordResetTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.PasswordResetTokenAsync(user));

        public ValueTask<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () => await this.userProcessingService.RegisterUserAsync(user, password));

        public ValueTask<bool> ResetUserPasswordByEmailOrUserNameAsync(ResetPasswordApiRequest resetPasswordApiRequest) =>
        TryCatch(async () => await this.userProcessingService.ResetUserPasswordByEmailOrUserNameAsync(resetPasswordApiRequest));

        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userProcessingService.RetrieveAllUsers());

        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(
            LoginCredentialsApiRequest loginCredentialsApiRequest) =>
        TryCatch(async () => 
        {
            return await this.userProcessingService.RetrieveUserByEmailOrUserNameAsync(loginCredentialsApiRequest);         
        });

        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail) =>
        TryCatch(async () => await this.userProcessingService.RetrieveUserByEmailOrUserNameAsync(userNameOrEmail));

        public ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.RetrieveUserRolesAsync(user));

        public ValueTask<string> TwoFactorTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.TwoFactorTokenAsync(user));
    }
}
