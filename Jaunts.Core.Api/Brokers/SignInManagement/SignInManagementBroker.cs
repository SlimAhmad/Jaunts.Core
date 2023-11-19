using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.SignInManagement
{
    public class SignInManagementBroker : ISignInManagementBroker
    {
        private readonly SignInManager<ApplicationUser> signInManagement;

        public SignInManagementBroker(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManagement = signInManager;

        }

        public async ValueTask SignOutAsync()
        {
            var broker = new SignInManagementBroker(this.signInManagement);

            await broker.signInManagement.SignOutAsync();
        }

        public async ValueTask<SignInResult> PasswordSignInAsync(ApplicationUser user,string password,bool isPersistent,bool lockoutOnFailure)
        {
            var broker = new SignInManagementBroker(this.signInManagement);

            return await broker.signInManagement.PasswordSignInAsync(user,password,isPersistent,lockoutOnFailure);
        }

        public async ValueTask<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        {
            var broker = new SignInManagementBroker(this.signInManagement);

            return await broker.signInManagement.TwoFactorSignInAsync(provider, code, isPersistent, rememberClient);
        }

    }
}
