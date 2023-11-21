using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.SignIn
{
    public partial interface ISignInProcessingService
    {
        ValueTask SignOutAsync();
        ValueTask<bool> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        ValueTask<bool> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
    }
}
