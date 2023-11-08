using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IUserManagementBroker userManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ApplicationUser> RegisterUserRequestAsync(ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnCreate(user, password);
            return await this.userManagementBroker.InsertUserAsync(user, password);
          
        });

        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userManagementBroker.SelectAllUsers());

        public ValueTask<ApplicationUser> RetrieveUserByIdRequestAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);
            ApplicationUser storageUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(storageUser, userId);

            return storageUser;
        });

        public ValueTask<ApplicationUser> ModifyUserRequestAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);
            ApplicationUser maybeUser = await this.userManagementBroker.SelectUserByIdAsync(user.Id);
            ValidateStorageUser(maybeUser, user.Id);
            ValidateAgainstStorageUserOnModify(inputUser: user, storageUser: maybeUser);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<ApplicationUser> RemoveUserByIdRequestAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);
            ApplicationUser maybeUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(maybeUser, userId);

            return await this.userManagementBroker.DeleteUserAsync(maybeUser);
        });
    }
}
