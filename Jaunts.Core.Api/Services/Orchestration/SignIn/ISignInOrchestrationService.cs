using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.SignIn
{
    public interface ISignInOrchestrationService
    {
        ValueTask SignOutAsync();
        ValueTask<bool> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        ValueTask<bool> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
        ValueTask<bool> LoginRequestAsync(
            string userNameOrEmail, string password);
        ValueTask<ApplicationUser> LoginOtpRequestAsync(
           string code, string userNameOrEmail);
    }
}
