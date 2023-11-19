using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

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
            var response =  await this.userManagementBroker.InsertUserAsync(user, password);
            ValidateIdentityResultResponse(response);
            return user;
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

        public ValueTask<string> GenerateEmailConfirmationTokenRequestAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.GenerateEmailConfirmationTokenAsync(user);
        });

        public ValueTask<string> GeneratePasswordResetTokenRequestAsync(ApplicationUser user)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.GeneratePasswordResetTokenAsync(user);
        });

        public ValueTask<string> GenerateTwoFactorTokenRequestAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.GenerateTwoFactorTokenAsync(user);
        });

        public ValueTask<bool> CheckPasswordRequestAsync(ApplicationUser user, string password)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await this.userManagementBroker.CheckPasswordAsync(user,password);
        });

        public ValueTask<ApplicationUser> ResetPasswordRequestAsync(ApplicationUser user, string token, string password)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateText(token);
            ValidateText(password);
            var result = await this.userManagementBroker.ResetPasswordAsync(user, token, password);
            ValidateIdentityResultResponse(result);
            return user;
        });

        public ValueTask<ApplicationUser> SetTwoFactorEnabledRequestAsync(ApplicationUser user, bool enabled)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var result = await this.userManagementBroker.SetTwoFactorEnabledAsync(user, enabled);
            ValidateIdentityResultResponse(result);
            return user;
        });

        public ValueTask<List<string>> RetrieveUserRolesRequestAsync(ApplicationUser user)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var result = await this.userManagementBroker.GetRolesAsync(user);
            return result.ToList();
        });

        public ValueTask<ApplicationUser> AddToRoleRequestAsync(ApplicationUser user, string role)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var response = await this.userManagementBroker.AddToRoleAsync(user,role);
            ValidateIdentityResultResponse(response);
            return user ;
        });

        public ValueTask<ApplicationUser> ConfirmEmailRequestAsync(ApplicationUser user, string token)=>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var result = await this.userManagementBroker.ConfirmEmailAsync(user,token);
            ValidateIdentityResultResponse(result);
            return await this.userManagementBroker.SelectUserByIdAsync(user.Id);
        });
    }
}
