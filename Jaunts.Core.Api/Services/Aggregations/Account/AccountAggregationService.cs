// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Aggregations.Account;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial class AccountAggregationService : IAccountAggregationService
    {
        private readonly IUserOrchestrationService  userOrchestrationService;
        private readonly ISignInManagementBroker signInManagementBroker;
        private readonly IEmailOrchestrationService emailOrchestrationService;
        private readonly IJwtOrchestrationService jwtOrchestrationService;
        private readonly ILoggingBroker loggingBroker;


        public AccountAggregationService(
            IUserOrchestrationService userOrchestrationService,
            IEmailOrchestrationService emailOrchestrationService,
            IJwtOrchestrationService jwtOrchestrationService,
            ILoggingBroker loggingBroker
           )
        {
            this.userOrchestrationService = userOrchestrationService;
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

            ApplicationUser registerUserResponse =
                await userOrchestrationService.RegisterUserAsync(
                    registerUserRequest,
                    registerCredentialsApiRequest.Password);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(registerUserResponse);
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
                await signInManagementBroker.SignOutAsync();
                await signInManagementBroker.PasswordSignInAsync(
                    user, loginCredentialsApiRequest.Password, false, true);
                var response = await emailOrchestrationService.TwoFactorMailAsync(user);

                if (response.Successful)
                    return ConvertTo2FAResponse(user);
            }
            ValidateUserResponse(user);
            var isValidPassword = await userOrchestrationService.CheckPasswordValidityAsync(
                loginCredentialsApiRequest.Password ,user.Id);
            return await jwtOrchestrationService.JwtAccountDetailsAsync(isValidPassword);
        });

        public ValueTask<bool> ResetPasswordRequestAsync(
            ResetPasswordApiRequest resetPassword) =>
        TryCatch(async () =>
        {
            return await userOrchestrationService.ResetUserPasswordByEmailOrUserNameAsync(resetPassword); 
        });

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
            await signInManagementBroker.TwoFactorSignInAsync(
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

    
        private UserAccountDetailsApiResponse ConvertTo2FAResponse(ApplicationUser user)
        {
            return new UserAccountDetailsApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
            };
        }


        private ApplicationUser ConvertToAuthRequest(RegisterUserApiRequest registerUserApiRequest)
        {
            return new ApplicationUser
            {
                UserName = registerUserApiRequest.Username,
                FirstName = registerUserApiRequest.FirstName,
                LastName = registerUserApiRequest.LastName,
                Email = registerUserApiRequest.Email,
                PhoneNumber = registerUserApiRequest.PhoneNumber
            };
        }
    }
}
