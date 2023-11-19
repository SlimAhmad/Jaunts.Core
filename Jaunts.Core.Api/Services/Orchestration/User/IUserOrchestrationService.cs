using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Orchestration.User
{
    public partial interface IUserOrchestrationService
    {
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password);
        ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user);
        ValueTask<string> PasswordResetTokenAsync(ApplicationUser user);
        ValueTask<string> TwoFactorTokenAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(LoginCredentialsApiRequest loginCredentialsApiRequest);
        ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail);
        ValueTask<bool> ResetUserPasswordByEmailOrUserNameAsync(ResetPasswordApiRequest resetPasswordApiRequest);
        ValueTask<bool> EnsureUserExistAsync(ApplicationUser user);
        ValueTask<ApplicationUser> EnableOrDisable2FactorAuthenticationAsync(Guid id);
        ValueTask<ApplicationUser> ConfirmEmailAsync(string token, string email);
        ValueTask<ApplicationUser> CheckPasswordValidityAsync(string password, Guid id);
        ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user);
    }
}
