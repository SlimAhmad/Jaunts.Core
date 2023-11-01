using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers.UserManagement
{


    public class UserManagementBroker : IUserManagementBroker
    {
        private readonly UserManager<ApplicationUser> userManagement;

        public UserManagementBroker(UserManager<ApplicationUser> userManager)
        {
            this.userManagement = userManager;
        }
        public IQueryable<ApplicationUser> SelectAllUsers() => this.userManagement.Users;

        public async ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByIdAsync(userId.ToString());
        }

        public async ValueTask<ApplicationUser> InsertUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);
            await broker.userManagement.CreateAsync(user, password);

            return user;
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
    }
}
