// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.SignIn;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial class AccountAggregationService : IAccountAggregationService
    {
        private readonly IUserOrchestrationService  userOrchestrationService;
        private readonly ISignInOrchestrationService signInOrchestrationService;
        private readonly IEmailOrchestrationService emailOrchestrationService;
        private readonly IJwtOrchestrationService jwtOrchestrationService;
        private readonly ILoggingBroker loggingBroker;


        public AccountAggregationService(
            IUserOrchestrationService userOrchestrationService,
            ISignInOrchestrationService signInOrchestrationService,
            IEmailOrchestrationService emailOrchestrationService,
            IJwtOrchestrationService jwtOrchestrationService,
            ILoggingBroker loggingBroker
           )
        {
            this.userOrchestrationService = userOrchestrationService;
            this.signInOrchestrationService= signInOrchestrationService;
            this.loggingBroker = loggingBroker;
            this.emailOrchestrationService= emailOrchestrationService;
            this.jwtOrchestrationService = jwtOrchestrationService;

        }

        public ValueTask<UserAccountDetailsResponse> RegisterUserRequestAsync(
            UserCredentialsRequest userCredentialsRequest) =>
        TryCatch(async () =>
        {
            ApplicationUser registerUserRequest =
                MapToUserRequest(userCredentialsRequest);
            ApplicationUser user =
                await userOrchestrationService.RegisterUserAsync(
                    registerUserRequest,
                    userCredentialsRequest.Password);
            var response = await jwtOrchestrationService.JwtAccountDetailsAsync(registerUserRequest);
            return response;
        });

        public ValueTask<UserAccountDetailsResponse> LogInRequestAsync(
            LoginRequest loginApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnLogin(loginApiRequest);
            ApplicationUser user = await userOrchestrationService.RetrieveUserByEmailOrUserNameAsync(
                 loginApiRequest.UsernameOrEmail);
                await signInOrchestrationService.LoginRequestAsync(
                    loginApiRequest.UsernameOrEmail,loginApiRequest.Password);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<bool> ResetPasswordRequestAsync(ResetPasswordRequest resetPassword) =>
        TryCatch(async () => await userOrchestrationService.ResetUserPasswordByEmailOrUserNameAsync(resetPassword));

        public ValueTask<bool> ForgotPasswordRequestAsync(string email) =>
        TryCatch(async () =>
        {
            ApplicationUser user = await userOrchestrationService.RetrieveUserByEmailOrUserNameAsync(email);
            var response = await emailOrchestrationService.PasswordResetMailAsync(user);
            return response.Successful;
        });

        public ValueTask<UserAccountDetailsResponse> EmailConfirmationAsync(string token, string email) =>
        TryCatch(async () =>
        {
            var user = await userOrchestrationService.ConfirmEmailAsync(token, email);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<UserAccountDetailsResponse> OtpLoginRequestAsync(
            string code, string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(userNameOrEmail);
            ValidateUserProfileDetails(code);
            ApplicationUser user = await signInOrchestrationService.LoginOtpRequestAsync(code, userNameOrEmail);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<UserAccountDetailsResponse> EnableUserTwoFactorAsync(Guid id) =>
        TryCatch(async () =>
        {
            var user = await userOrchestrationService.EnableOrDisableTwoFactorAsync(id);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        private ApplicationUser MapToUserRequest(UserCredentialsRequest registerUserApiRequest)
        {
            return new ApplicationUser
            {
                UserName = registerUserApiRequest.Username,
                FirstName = registerUserApiRequest.FirstName,
                LastName = registerUserApiRequest.LastName,
                Email = registerUserApiRequest.Email,
                PhoneNumber = registerUserApiRequest.PhoneNumber,
                CreatedDate = registerUserApiRequest.CreatedDate,
                UpdatedDate = registerUserApiRequest.UpdatedDate,
                ConcurrencyStamp = null
            };
        }
    }
}
