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
using Microsoft.AspNetCore.Identity;

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

        public ValueTask<UserAccountDetailsApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ApplicationUser registerUserRequest =
                ConvertToAuthRequest(registerCredentialsApiRequest);
            var registerUserResponse =
                await userOrchestrationService.RegisterUserAsync(
                    registerUserRequest,
                    registerCredentialsApiRequest.Password);
            await userOrchestrationService.AddUserToRoleAsync(registerUserResponse, "User");
            await emailOrchestrationService.VerificationMailAsync(registerUserResponse);
            var response = await jwtOrchestrationService.JwtAccountDetailsAsync(registerUserResponse);
            return response;
        });

        public ValueTask<UserAccountDetailsApiResponse> LogInRequestAsync(
            LoginCredentialsApiRequest loginCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnLogin(loginCredentialsApiRequest);
            ApplicationUser user = await userOrchestrationService.RetrieveUserByEmailOrUserNameAsync(
                loginCredentialsApiRequest);

            if (user.TwoFactorEnabled)
            {
                await signInOrchestrationService.SignOutAsync();
                await signInOrchestrationService.PasswordSignInAsync(
                    user, loginCredentialsApiRequest.Password, false, true);
               return await emailOrchestrationService.TwoFactorMailAsync(user);

            }
            var isValidPassword = await userOrchestrationService.CheckPasswordValidityAsync(
                loginCredentialsApiRequest.Password ,user.Id);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<bool> ResetPasswordRequestAsync(ResetPasswordApiRequest resetPassword) =>
        TryCatch(async () => await userOrchestrationService.ResetUserPasswordByEmailOrUserNameAsync(resetPassword));

        public ValueTask<bool> ForgotPasswordRequestAsync(string email) =>
        TryCatch(async () =>
        {
            ApplicationUser user = await userOrchestrationService.RetrieveUserByEmailOrUserNameAsync(email);
            var response = await emailOrchestrationService.PasswordResetMailAsync(user);
            return response.Successful;
        });

        public ValueTask<UserAccountDetailsApiResponse> ConfirmEmailRequestAsync(string token, string email) =>
        TryCatch(async () =>
        {
            var user = await userOrchestrationService.ConfirmEmailAsync(token, email);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<UserAccountDetailsApiResponse> LoginWithOTPRequestAsync(
            string code, string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(userNameOrEmail);
            ValidateUserProfileDetails(code);
            await signInOrchestrationService.TwoFactorSignInAsync(
                TokenOptions.DefaultPhoneProvider, code, false, false);
            ApplicationUser user = await userOrchestrationService.RetrieveUserByEmailOrUserNameAsync(userNameOrEmail);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        public ValueTask<UserAccountDetailsApiResponse> EnableUser2FARequestAsync(Guid id) =>
        TryCatch(async () =>
        {
            var user = await userOrchestrationService.EnableOrDisable2FactorAuthenticationAsync(id);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(user);
        });

        private ApplicationUser ConvertToAuthRequest(RegisterUserApiRequest registerUserApiRequest)
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
