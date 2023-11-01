using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Brokers.UserManagement
{
    public partial class UserManagementBroker
    {

        public IQueryable<ApplicationUser> SelectAllUsers() => this.userManagement.Users;
        public async ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByIdAsync(userId.ToString());
        }
        public async ValueTask<ApplicationUser> SelectUserByEmailAsync(Guid userId)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByEmailAsync(userId.ToString());
        }
        public async ValueTask<ApplicationUser> InsertUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);
            var i = await broker.userManagement.CreateAsync(user, password);

            return user;
        }
        public async ValueTask<ApplicationUser> SelectUser(HttpContext context)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.GetUserAsync(context.User);
        }
        public async ValueTask<ApplicationUser> SelectByUserName(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.FindByNameAsync(user.UserName);
        }
        public async ValueTask<IQueryable<ApplicationUser>> SelectUsersRolesAsync(string role)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return (IQueryable<ApplicationUser>)await broker.userManagement.GetUsersInRoleAsync(role);

        }
        public async ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            await broker.userManagement.UpdateAsync(user);

            return user;
        }
        public async ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            await broker.userManagement.DeleteAsync(user);

            return user;
        }
        public async ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.ConfirmEmailAsync(user, token);

        }
        public async ValueTask<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.ResetPasswordAsync(user, token, newPassword);

        }
        public async ValueTask<string> GetChangeEmailTokenAsync(ApplicationUser user, string newEmail)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.GenerateChangeEmailTokenAsync(user, newEmail);

        }
        public async ValueTask<string> GetPasswordResetTokenAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.GeneratePasswordResetTokenAsync(user);

        }
        public async ValueTask<bool> VerifyTokenAsync(ApplicationUser user, string tokenProvider, string purpose, string token)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

        }
    }
}
