using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;

namespace Jaunts.Core.Api.Services.Processings.User
{
    public partial interface IUserProcessingService
    {
        ValueTask<ApplicationUser> UpsertUserAsync(
           ApplicationUser user, string password);
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
        ValueTask<ApplicationUser> RetrieveUserById(
           Guid id);
        ValueTask<bool> RemoveUserByIdAsync(
           Guid id);
        ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user);
        ValueTask<string> PasswordResetTokenAsync(ApplicationUser user);
        ValueTask<string> RetrieveTwoFactorTokenAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail);
        ValueTask<bool> ResetUserPasswordByEmailAsync(string email, string token, string password);
        ValueTask<bool> EnsureUserExistAsync(ApplicationUser user);
        ValueTask<ApplicationUser> EnableOrDisableTwoFactorAsync(Guid id);
        ValueTask<ApplicationUser> ValidateEmailTokenAsync(string token, string email);
        ValueTask<bool> ValidatePasswordAsync(string password, Guid id);
        ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user);
        ValueTask<ApplicationUser> AddToRoleAsync(ApplicationUser user, string role);
    }
}
