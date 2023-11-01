using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.UserManagement
{
    public interface IUserManagementBroker
    {
        ValueTask<ApplicationUser> InsertUserAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> SelectAllUsers();
        ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId);
        ValueTask<ApplicationUser> SelectUserByEmailAsync(Guid userId);
        ValueTask<ApplicationUser> SelectUser(HttpContext user);
        ValueTask<IQueryable<ApplicationUser>> SelectUsersRolesAsync(string role);
        ValueTask<ApplicationUser> SelectByUserName(ApplicationUser user);
        ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user);
        ValueTask<bool> VerifyTokenAsync(ApplicationUser user, string tokenProvider, string purpose, string token);
        ValueTask<string> GetPasswordResetTokenAsync(ApplicationUser user);
        ValueTask<string> GetChangeEmailTokenAsync(ApplicationUser user, string newEmail);
        ValueTask<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    }
}
