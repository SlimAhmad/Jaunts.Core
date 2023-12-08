using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.SignIn;

namespace Jaunts.Core.Api.Services.Processings.SignIn
{
    public partial class SignInProcessingService : ISignInProcessingService
    {
      
            private readonly ISignInService SignInService;
            private readonly ILoggingBroker loggingBroker;

            public SignInProcessingService(
                ISignInService  signInService,
                ILoggingBroker loggingBroker

                )
            {
                this.SignInService = signInService;
                this.loggingBroker = loggingBroker;

            }


        public ValueTask<bool> PasswordSignInAsync(
            ApplicationUser user,string password,
            bool isPersistent, bool lockoutOnFailure) =>
        TryCatch(async () =>
        {
           ValidateUser(user);
           ValidateString(password);
           var response = await SignInService.PasswordSignInRequestAsync(user, password,isPersistent,lockoutOnFailure);
           return response.Succeeded;
        });

        public ValueTask SignOutAsync() =>
        TryCatch(async () =>
        {
            await SignInService.SignOutRequestAsync();

        });

        public ValueTask<bool> TwoFactorSignInAsync(
            string provider, string code, 
            bool isPersistent, bool rememberClient) =>
        TryCatch(async () =>
        {
            ValidateString(provider);
            ValidateString(code);
            var response = await SignInService.TwoFactorSignInRequestAsync(provider, code,isPersistent,rememberClient);
             return response.Succeeded;   
        });
    }
}
