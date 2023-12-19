// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq.Expressions;
using System.Web;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Processings.User
{
    public partial class UserProcessingService : IUserProcessingService
    {
        private readonly IUserService userService;
        private readonly ILoggingBroker loggingBroker;

        public UserProcessingService(
            IUserService userService,
            ILoggingBroker loggingBroker)
        {
            this.userService = userService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ApplicationUser> UpsertUserAsync(
           ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateStringIsNotNull(password);
            ApplicationUser maybeUser = RetrieveMatchingUser(user);

            return maybeUser switch
            {
                null => await this.userService.AddUserAsync(user, password),
                _ => await this.userService.ModifyUserAsync(user)
            };
        });

        public ValueTask<ApplicationUser> RetrieveUserById(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            ApplicationUser user = await userService.RetrieveUserByIdAsync(id);
            ValidateUser(user);

            return user;
        });

        public ValueTask<bool> RemoveUserByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            ApplicationUser user = await userService.RemoveUserByIdAsync(id);
            ValidateUser(user);

            return true;
        });

        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userService.RetrieveAllUsers());
        
        public ValueTask<ApplicationUser> CreateUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () => await this.userService.AddUserAsync(user, password));
        
        public ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.RetrieveUserEmailConfirmationTokenAsync(user));
        
        public ValueTask<string> PasswordResetTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.RetrieveUserPasswordTokenAsync(user));
        
        public ValueTask<string> RetrieveTwoFactorTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.RetrieveUserTwoFactorTokenAsync(user));
        
        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateStringIsNotNull(userNameOrEmail);
            var user = userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(userNameOrEmail));
            
            return user;
        });
        
        public ValueTask<bool> ResetUserPasswordByEmailAsync(string email, string token, string password) =>
        TryCatch(async () =>
        {
            ValidateStringIsNotNull(email);
            ValidateStringIsNotNull(token);
            ValidateStringIsNotNull(password);
            var user = userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(email));
            ValidateUserResponseIsNotNull(user);
            var passwordReset = await userService.ModifyUserPasswordAsync(
                user, HttpUtility.UrlDecode(token), password);
            var response = passwordReset != null ? true : false;
            
            return response;
        });
        
        public ValueTask<bool> EnsureUserExistAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var allUsers = userService.RetrieveAllUsers().ToList();
            
            return allUsers.Any(retrievedUser => retrievedUser.Id == user.Id);

        });
        
        public ValueTask<ApplicationUser> EnableOrDisableTwoFactorAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            IQueryable<ApplicationUser> allUser =
                this.userService.RetrieveAllUsers();

            ApplicationUser user = allUser.FirstOrDefault(
                      SameUserAs(id));

            ApplicationUser enabledOrDisabledUser = user.TwoFactorEnabled switch
            {
                false => await this.userService.ModifyUserTwoFactorAsync(user, true),
                _ => await this.userService.ModifyUserTwoFactorAsync(user, false)
            };
            
            return userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(enabledOrDisabledUser.Email));

        });
        
        public ValueTask<ApplicationUser> ValidateEmailTokenAsync(string token, string email) =>
        TryCatch(async () =>
        {
            ValidateStringIsNotNull(token);
            ValidateStringIsNotNull(email);
            var user = userService.RetrieveAllUsers().FirstOrDefault(SameUserAs(email));
            ValidateUser(user);
            
            return await userService.ValidateEmailTokenAsync(user, token);

        });
        
        public ValueTask<bool> ValidatePasswordAsync(string password, Guid id) =>
        TryCatch(async () =>
        {
            ValidateStringIsNotNull(password);
            ValidateUserId(id);
            ValidateStringIsNotNull(password);
            var user = userService.RetrieveAllUsers()
                     .FirstOrDefault(SameUserAs(id));
            ValidateUser(user);
            
            return await userService.ValidatePasswordAsync(user, password);

        });
        
        public ValueTask<ApplicationUser> AddToRoleAsync(ApplicationUser user, string role) =>
        TryCatch(() => userService.AddUserRolesAsync(user, role));
        
        public ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            
            return await userService.RetrieveUserRolesAsync(user);
        });


        private ApplicationUser RetrieveMatchingUser(ApplicationUser user)
        {
            IQueryable<ApplicationUser> users =
            this.userService.RetrieveAllUsers();
            
            return users.FirstOrDefault(SameUserAs(user));
        }
        
        private static Expression<Func<ApplicationUser, bool>> SameUserAs(Guid id) =>
             retrievedUser => retrievedUser.Id == id;
        
        private static Expression<Func<ApplicationUser, bool>> SameUserAs(ApplicationUser user) =>
             retrievedUser => retrievedUser.Id == user.Id;
        
        private static Expression<Func<ApplicationUser, bool>> SameUserAs(string IdOrUserNameOrEmail)
        {
            return retrievedUser => retrievedUser.Email == IdOrUserNameOrEmail ||
                                    retrievedUser.Id.ToString() == IdOrUserNameOrEmail ||
                                    retrievedUser.UserName == IdOrUserNameOrEmail;
        }
    }
}