// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.SignIn;
using Jaunts.Core.Api.Services.Processings.User;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Orchestration.SignIn
{
    public partial class SignInOrchestrationService : ISignInOrchestrationService
    {
        private readonly ISignInProcessingService  signInProcessingService;
        private readonly IEmailProcessingService emailProcessingService;
        private readonly IUserProcessingService userProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public SignInOrchestrationService(
            ISignInProcessingService  signInProcessingService,
            IEmailProcessingService emailProcessingService,
            IUserProcessingService userProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.signInProcessingService = signInProcessingService;
            this.emailProcessingService = emailProcessingService;
            this.userProcessingService = userProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> PasswordSignInAsync(
            ApplicationUser user, string password,
            bool isPersistent, bool lockoutOnFailure) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateString(password);
           return await signInProcessingService.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        });


        public ValueTask SignOutAsync() =>
        TryCatch(async () => await signInProcessingService.SignOutAsync());

        public ValueTask<bool> TwoFactorSignInAsync(
            string provider, string code,
            bool isPersistent, bool rememberClient) =>
        TryCatch(async () =>
        {
            ValidateString(provider);
            ValidateString(code);
            return await signInProcessingService.TwoFactorSignInAsync(provider,code,isPersistent,rememberClient);
        });


        public ValueTask<bool> TwoFactorLoginRequestAsync(
            ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
                await signInProcessingService.SignOutAsync();
                await signInProcessingService.PasswordSignInAsync(
                    user, password, false, true);
                string token = await userProcessingService.EmailConfirmationTokenAsync(user);
                var response = await emailProcessingService.SendOtpVerificationMailRequestAsync(user, token);
                if (response is null)
                    return false;
                return true;
        });

        public ValueTask<ApplicationUser> LoginOtpRequestAsync(
           string code, string userNameOrEmail) =>
       TryCatch(async () =>
       {
           await signInProcessingService.TwoFactorSignInAsync(
               TokenOptions.DefaultPhoneProvider, code, false, false);
           return await userProcessingService.RetrieveUserByEmailOrUserNameAsync(userNameOrEmail);
       });
    }
}
