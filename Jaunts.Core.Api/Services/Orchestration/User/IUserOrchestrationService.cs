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
        ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail);
        ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid userId);
        ValueTask<bool> ResetUserPasswordByEmailOrUserNameAsync(ResetPasswordRequest resetPasswordApiRequest);
        ValueTask<bool> EnsureUserExistAsync(ApplicationUser user);
        ValueTask<ApplicationUser> EnableOrDisableTwoFactorAsync(Guid id);
        ValueTask<ApplicationUser> ConfirmEmailAsync(string token, string email);
        ValueTask<bool> CheckPasswordValidityAsync(string password, Guid id);
        ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user);
        ValueTask<ApplicationUser> AddUserToRoleAsync(ApplicationUser user, string role);
        ValueTask<bool> RemoveUserByIdAsync(Guid id);
        ValueTask<ApplicationUser> UpsertUserAsync(ApplicationUser user, string password);
    }
}
