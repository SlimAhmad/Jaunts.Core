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

        public async ValueTask<IdentityResult> InsertUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);
            return await broker.userManagement.CreateAsync(user, password);  
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

        public async ValueTask<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.GenerateEmailConfirmationTokenAsync(user);
        }

        public async ValueTask<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.GeneratePasswordResetTokenAsync(user);
        }
        public async ValueTask<ApplicationUser> FindByEmailAsync(string email)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByEmailAsync(email);
        }

        public async ValueTask<ApplicationUser> FindByNameAsync(string userName)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByNameAsync(userName);
        }

        public async ValueTask<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.CreateAsync(user,password);
        }

        public async ValueTask<string> GenerateTwoFactorTokenAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
        }

        public async ValueTask<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.CheckPasswordAsync(user, password);
        }

        public async ValueTask<IdentityResult> ResetPasswordAsync(ApplicationUser user,string token, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.ResetPasswordAsync(user,token,password);
        }

        public async ValueTask<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.SetTwoFactorEnabledAsync(user, enabled);
        }

        public async ValueTask<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.GetRolesAsync(user);
        }

        public async ValueTask<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.AddToRoleAsync(user,role);
        }

        public async ValueTask<ApplicationUser> FindByIdAsync(string id)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByIdAsync(id);
        }

        public async ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user,string token)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.ConfirmEmailAsync(user,token);
        }
    }
}
