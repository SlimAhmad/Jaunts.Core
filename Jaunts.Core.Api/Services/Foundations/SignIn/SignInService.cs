using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.SignIn
{
    public partial class SignInService : ISignInService
    {

        private readonly ISignInManagementBroker signInManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SignInService(
            SignInManagementBroker signInManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.signInManagementBroker = signInManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SignInResult> PasswordSignInRequestAsync(
            ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateText(password);
            return await this.signInManagementBroker.PasswordSignInAsync(user,password,isPersistent,lockoutOnFailure);
     
        });

        public async ValueTask SignOutRequestAsync() =>
            await this.signInManagementBroker.SignOutAsync();
     
        public ValueTask<SignInResult> TwoFactorSignInRequestAsync(
            string provider, string code, bool isPersistent, bool rememberClient) =>
        TryCatch(async () =>
        {
            ValidateText(provider);                 
            ValidateText(code);
            return await this.signInManagementBroker.TwoFactorSignInAsync(provider,code,isPersistent,rememberClient);
      
        });
    }
}
