using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.SignIn
{
    public partial interface ISignInService
    {
        ValueTask SignOutRequestAsync();
        ValueTask<SignInResult> PasswordSignInRequestAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        ValueTask<SignInResult> TwoFactorSignInRequestAsync(string provider, string code, bool isPersistent, bool rememberClient);
    }
}
