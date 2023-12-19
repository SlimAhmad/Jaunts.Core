// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid userId);
        ValueTask<ApplicationUser> ModifyUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RemoveUserByIdAsync(Guid userId);

        ValueTask<string> RetrieveUserEmailConfirmationTokenAsync(ApplicationUser user);
        ValueTask<string> RetrieveUserPasswordTokenAsync(ApplicationUser user);
        ValueTask<string> RetrieveUserTwoFactorTokenAsync(ApplicationUser user);
        ValueTask<ApplicationUser> ModifyUserPasswordAsync(ApplicationUser user, string token, string password);
        ValueTask<ApplicationUser> ModifyUserTwoFactorAsync(ApplicationUser user, bool enabled);
        ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user);
        ValueTask<ApplicationUser> AddUserRolesAsync(ApplicationUser user, string role);
        
        ValueTask<bool> ValidatePasswordAsync(ApplicationUser user, string password);
        ValueTask<ApplicationUser> ConfirmEmailAsync(ApplicationUser user, string token);
    }
}