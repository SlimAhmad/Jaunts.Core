// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Processings.SignIn;

namespace Jaunts.Core.Api.Services.Orchestration.SignIn
{
    public partial class SignInOrchestrationService : ISignInOrchestrationService
    {
        private readonly ISignInProcessingService  signInProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public SignInOrchestrationService(
            ISignInProcessingService  signInProcessingService,
            ILoggingBroker loggingBroker
           )
        {
            this.signInProcessingService = signInProcessingService;
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

  

    }
}
