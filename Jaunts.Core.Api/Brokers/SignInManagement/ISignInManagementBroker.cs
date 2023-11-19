using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.SignInManagement
{
    public interface ISignInManagementBroker
    {
        ValueTask SignOutAsync();
        ValueTask<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        ValueTask<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
    }
}
