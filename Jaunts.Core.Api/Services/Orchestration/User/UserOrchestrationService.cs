using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.Jwt;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.User
{
    public partial class UserOrchestrationService : IUserOrchestrationService
    {
        private readonly IUserProcessingService userProcessingService;
        private readonly IEmailProcessingService emailProcessingService;
        private readonly ILoggingBroker loggingBroker;


        public UserOrchestrationService(
            IUserProcessingService userProcessingService,
            IEmailProcessingService emailProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.userProcessingService = userProcessingService;
            this.emailProcessingService = emailProcessingService;
            this.loggingBroker = loggingBroker;

        }

        #region CRUD

        public ValueTask<ApplicationUser> UpsertUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () => await this.userProcessingService.UpsertUserAsync(user, password));
        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userProcessingService.RetrieveAllUsers());
        public ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () => await this.userProcessingService.RetrieveUserById(userId));

        #endregion

        #region Miscellaneous

        public ValueTask<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
            var registerUserResponse =
                await userProcessingService.CreateUserAsync(
                    user,
                    password);
            await userProcessingService.AddToRoleAsync(registerUserResponse, "User");
            string token = await userProcessingService.EmailConfirmationTokenAsync(user);
            await emailProcessingService.VerificationMailRequestAsync(registerUserResponse,token);   
            return user;
        });
        public ValueTask<bool> CheckPasswordValidityAsync(string password, Guid id) =>
        TryCatch(async () => await this.userProcessingService.ValidatePasswordAsync(password,id));
        public ValueTask<ApplicationUser> ConfirmEmailAsync(string token, string email) =>
        TryCatch(async () => await this.userProcessingService.ConfirmEmailAsync(token,email));
        public ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.EmailConfirmationTokenAsync(user));
        public ValueTask<ApplicationUser> EnableOrDisableTwoFactorAsync(Guid id) =>
        TryCatch(async () => await this.userProcessingService.ModifyTwoFactorAsync(id));
        public ValueTask<bool> EnsureUserExistAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.EnsureUserExistAsync(user));
        public ValueTask<string> PasswordResetTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.PasswordResetTokenAsync(user));
        public ValueTask<bool> ResetUserPasswordByEmailOrUserNameAsync(ResetPasswordRequest resetPasswordApiRequest) =>
        TryCatch(async () => 
        {
           return await this.userProcessingService.ResetUserPasswordByEmailAsync(
                resetPasswordApiRequest.Email, resetPasswordApiRequest.Token, resetPasswordApiRequest.Password);
        });
        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail) =>
        TryCatch(async () => await this.userProcessingService.RetrieveUserByEmailOrUserNameAsync(userNameOrEmail));
        public ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.RetrieveUserRolesAsync(user));
        public ValueTask<string> TwoFactorTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userProcessingService.RetrieveTwoFactorTokenAsync(user));
        public ValueTask<ApplicationUser> AddUserToRoleAsync(ApplicationUser user,string role) =>
        TryCatch(async () => await this.userProcessingService.AddToRoleAsync(user,role));
        public ValueTask<bool> RemoveUserByIdAsync(Guid id) =>
        TryCatch(async () => await this.userProcessingService.RemoveUserByIdAsync(id));

        #endregion
      

    }
}
