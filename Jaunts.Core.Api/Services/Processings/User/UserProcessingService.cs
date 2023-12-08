using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Web;

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
            ApplicationUser maybeUser = RetrieveMatchingUser(user);

            return maybeUser switch
            {
                null => await this.userService.InsertUserRequestAsync(user, password),
                _ => await this.userService.ModifyUserRequestAsync(user)
            };
        });
        public ValueTask<ApplicationUser> RetrieveUserById(
           Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            ApplicationUser user = await userService.RetrieveUserByIdRequestAsync(id);
            ValidateUser(user);
            return user;
        });
        public ValueTask<bool> RemoveUserByIdAsync(
           Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            ApplicationUser user = await userService.RemoveUserByIdRequestAsync(id);
            ValidateUser(user);
            return true;
        });
        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userService.RetrieveAllUsers());
        public ValueTask<ApplicationUser> RegisterUserAsync(ApplicationUser user,string password) =>
        TryCatch(async() => await this.userService.InsertUserRequestAsync(user,password));
        public ValueTask<string> EmailConfirmationTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.GenerateEmailConfirmationTokenRequestAsync(user));
        public ValueTask<string> PasswordResetTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.GeneratePasswordResetTokenRequestAsync(user));
        public ValueTask<string> TwoFactorTokenAsync(ApplicationUser user) =>
        TryCatch(async () => await this.userService.GenerateTwoFactorTokenRequestAsync(user));
        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(LoginCredentialsApiRequest loginCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserLoginIsNotNull(loginCredentialsApiRequest);
            var user =  userService.RetrieveAllUsers();
            return user.FirstOrDefault(
                SameUserAs(loginCredentialsApiRequest.UsernameOrEmail));
            
        });
        public ValueTask<ApplicationUser> RetrieveUserByEmailOrUserNameAsync(string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserEmailOrUsername(userNameOrEmail);
            var user = userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(userNameOrEmail));
            return user;
        });
        public ValueTask<bool> ResetUserPasswordByEmailAsync(ResetPasswordApiRequest resetPasswordApiRequest) =>
        TryCatch(async () =>
        { 
            ValidateResetPasswordIsNull(resetPasswordApiRequest);
            var user =  userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(resetPasswordApiRequest.Email));
            ValidateUserResponseIsNotNull(user);
            var passwordReset = await userService.ResetPasswordRequestAsync(
                user, HttpUtility.UrlDecode(resetPasswordApiRequest.Token), resetPasswordApiRequest.Password);
            var response = passwordReset != null? true : false;
            return response;
        });
        public ValueTask<bool> EnsureUserExistAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            var allUsers =  userService.RetrieveAllUsers().ToList();
            return allUsers.Any(retrievedUser => retrievedUser.Id == user.Id);
          
        });
        public ValueTask<ApplicationUser> EnableOrDisable2FactorAuthenticationAsync(Guid id) =>
        TryCatch(async () =>
        {
           
            IQueryable<ApplicationUser> allUser =
                this.userService.RetrieveAllUsers();

            bool userExists = allUser.Any(retrievedStudent =>
                retrievedStudent.Id == id);

            ApplicationUser user =  allUser.FirstOrDefault(
                      SameUserAs(id));

           ApplicationUser enabledOrDisabledUser = userExists switch
            {
                false => await this.userService.SetTwoFactorEnabledRequestAsync(user, true),
                _ => await this.userService.SetTwoFactorEnabledRequestAsync(user, false)
            };
            return userService.RetrieveAllUsers().FirstOrDefault(
                SameUserAs(enabledOrDisabledUser.Email));

        });
        public ValueTask<ApplicationUser> ConfirmEmailAsync(string token, string email) =>
        TryCatch(async () =>
        { 
            var user = userService.RetrieveAllUsers()
                        .FirstOrDefault(SameUserAs(email));
            ValidateUser(user);
            return await userService.ConfirmEmailRequestAsync(user,token);

        });
        public ValueTask<bool> CheckPasswordValidityAsync(string password, Guid id) =>
        TryCatch(async () =>
        {
            var user =  userService.RetrieveAllUsers()
                     .FirstOrDefault(SameUserAs(id));
            ValidateUser(user);
            return await userService.CheckPasswordRequestAsync(user, password);

        });
        public ValueTask<ApplicationUser> AddToRoleAsync(ApplicationUser user, string role) =>
        TryCatch(() => userService.AddToRoleRequestAsync(user, role));
        public ValueTask<List<string>> RetrieveUserRolesAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            return await userService.RetrieveUserRolesRequestAsync(user);
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
