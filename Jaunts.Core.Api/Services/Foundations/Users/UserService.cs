// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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

        #region CRUD IMPLEMENTATION

        public ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnAdd(user, password);

            var userAddResponse =
            await this.userManagementBroker.InsertUserAsync(user, password);

            //Why do we need this validation here
            ValidateUserOnAddResponse(userAddResponse);
            //Why do we need to return the user
            return user;
        });

        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userManagementBroker.SelectAllUsers());

        public ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);

            ApplicationUser maybeUser =
                await this.userManagementBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return maybeUser;
        });

        public ValueTask<ApplicationUser> ModifyUserAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);

            ApplicationUser maybeUser =
                await this.userManagementBroker.SelectUserByIdAsync(user.Id);

            ValidateStorageUser(maybeUser, user.Id);

            ValidateAgainstStorageUserOnModify(
                inputUser: user, storageUser: maybeUser);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<ApplicationUser> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);

            ApplicationUser maybeUser =
                await this.userManagementBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return await this.userManagementBroker.DeleteUserAsync(maybeUser);
        });
        #endregion

        #region FINISH FOUNDATION

        public ValueTask<string> RetrieveUserEmailConfirmationTokenAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            string token =
                await this.userManagementBroker.GenerateEmailConfirmationTokenAsync(user);
            return token;
        });
        public ValueTask<string> RetrieveUserPasswordTokenAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.GeneratePasswordResetTokenAsync(user);
        });
        public ValueTask<string> RetrieveUserTwoFactorTokenAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.GenerateTwoFactorTokenAsync(user);
        });
        public ValueTask<ApplicationUser> ModifyUserPasswordAsync(
            ApplicationUser user, string token, string password) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateText(token);
            ValidateText(password);

            var result =
                await this.userManagementBroker.ResetPasswordAsync(user, token, password);

            ValidateUserOnAddResponse(result);

            return user;
        });

        public ValueTask<ApplicationUser> ModifyUserTwoFactorAsync(ApplicationUser user, bool enabled) =>
        TryCatch(async () =>
        {
            ValidateUser(user);

            var result =
            await this.userManagementBroker.SetTwoFactorEnabledAsync(user, enabled);

            ValidateUserOnAddResponse(result);

            return user;
        });

        public ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var result =
            await this.userManagementBroker.GetRolesAsync(user);
            return result.ToList();
        });

        public ValueTask<ApplicationUser> AddUserRolesAsync(ApplicationUser user, string role) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateText(role);
            var response =
                await this.userManagementBroker.AddToRoleAsync(user, role);

            ValidateUserOnAddResponse(response);

            return user;
        });

        #endregion

    }
}