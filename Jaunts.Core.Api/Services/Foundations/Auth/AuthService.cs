using Jaunts.Core.Api.Authorization;
using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Authorization;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Jaunts.Core.Api.Services.Foundations.Auth
{
    public partial class AuthService : IAuthService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleManagementBroker roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AuthService(
            IUserManagementBroker userManagementBroker,
            UserManager<ApplicationUser> userManager,
            IRoleManagementBroker roleManager,
            IEmailService emailService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }



        public ValueTask<RegisterResultApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnRegister(registerCredentialsApiRequest);
            ApplicationUser  registerUserRequest = ConvertAuthRequest(registerCredentialsApiRequest);
            ApplicationUser registerUserResponse =  await this.userManagementBroker.InsertUserAsync(registerUserRequest, registerCredentialsApiRequest.Password);
            ValidateUserOnRegisterResponseIsNull(registerUserResponse);
            return await ConvertToRegisterResponse(registerUserResponse);
        });
        public ValueTask<UserProfileDetailsApiResponse> LogInRequestAsync(
            LoginCredentialsApiRequest loginCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnLogin(loginCredentialsApiRequest);
            var isEmail = loginCredentialsApiRequest.UsernameOrEmail.Contains("@");
            var user = isEmail ?
                await userManager.FindByEmailAsync(loginCredentialsApiRequest.UsernameOrEmail) :
                await userManager.FindByNameAsync(loginCredentialsApiRequest.UsernameOrEmail);
            ValidateUserResponse(user);
            return await ConvertToLoginResponse(user);
        });
        public  ValueTask<ResetPasswordApiResponse> ResetPasswordRequestAsync(ResetPasswordApiRequest resetPassword) =>
        TryCatch(async () =>
        {
            ValidateResetPassword(resetPassword);
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            ValidateUserResponse(user);
            var response = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            ValidateIdentityResultResponse(response);
            return ConvertToResetPasswordResponse(user);
        });
        public ValueTask<ForgotPasswordApiResponse> ForgotPasswordRequestAsync(string email) =>
        TryCatch(async () =>
        {
            ValidateUserEmail(email);
            var user = await userManager.FindByEmailAsync(email);
            ValidateUserResponse(user);
            return await ConvertToForgotPasswordResponse(user);

        });
        public ValueTask<UserProfileDetailsApiResponse> ConfirmEmailRequestAsync(string token, string email) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(token);
            ValidateUserProfileDetails(email);
            var user = await userManager.FindByEmailAsync(email);
            ValidateUserResponse(user);
            var identityResult = await userManager.ConfirmEmailAsync(user, email);
            ValidateIdentityResultResponse(identityResult);
            return await ConvertToConfirmEmailResponse(user);
        });
        public ValueTask<UserProfileDetailsApiResponse> LoginWithOTPRequestAsync(string code, string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(userNameOrEmail);
            ValidateUserProfileDetails(code);
            var signIn = signInManager.TwoFactorSignInAsync("Email", code, false, false);
            var isEmail = userNameOrEmail.Contains("@");
            var user = isEmail ?
               await userManager.FindByEmailAsync(userNameOrEmail) :
               await userManager.FindByNameAsync(userNameOrEmail);
            ValidateUserResponse(user);
            return await ConvertToLoginResponse(user);
        });

        public ValueTask<UserProfileDetailsApiResponse> EnableUser2FARequestAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            var user = await userManager.FindByIdAsync(id.ToString());
            ValidateUserResponse(user);
            var twoFactorEnabled = await signInManager.IsTwoFactorEnabledAsync(user);
            var enable = twoFactorEnabled ?
               await userManager.SetTwoFactorEnabledAsync(user,true) :
               await userManager.SetTwoFactorEnabledAsync(user,false);
         
            return await ConvertToLoginResponse(user);
        });

        private async ValueTask<UserProfileDetailsApiResponse> ConvertToLoginResponse(ApplicationUser user)
        {

            var role = await userManager.GetRolesAsync(user);
            var userRoles = await roleManager.SelectAllRoles().Where(r => role.Contains(r.Name!)).ToListAsync();

            //
            var userPermissions = Permissions.None;
            //
            foreach (var rolePermission in userRoles)
                userPermissions |= rolePermission.Permissions;

            //
            var permissionsValue = (int)userPermissions;

            return new UserProfileDetailsApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = user.GenerateJwtToken(permissionsValue),
                Role = (List<string>)role
            };
            
        }
        private async ValueTask<RegisterResultApiResponse> ConvertToRegisterResponse(ApplicationUser user)
        {

            await userManager.AddToRoleAsync(user, "User");

            var role = await userManager.GetRolesAsync(user);

            //
            var userRoles = await roleManager.SelectAllRoles().Where(r => role.Contains(r.Name!)).ToListAsync();

            //
            var userPermissions = Permissions.None;
            //
            foreach (var rolePermission in userRoles)
                userPermissions |= rolePermission.Permissions;

            //
            var permissionsValue = (int)userPermissions;

            await emailService.PostVerificationMailRequestAsync(user, "Verify Your Email - Jaunts");

            return new RegisterResultApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = user.GenerateJwtToken(permissionsValue),
                Role = (List<string>)role,
                Username = user.UserName,
            };
            
        }
        private ApplicationUser ConvertAuthRequest(RegisterUserApiRequest registerCredentialsApiRequest)
        {
            return new ApplicationUser
            {
                UserName = registerCredentialsApiRequest.Username,
                FirstName = registerCredentialsApiRequest.FirstName,
                LastName = registerCredentialsApiRequest.LastName,
                Email = registerCredentialsApiRequest.Email,
                PhoneNumber = registerCredentialsApiRequest.PhoneNumber
            };
        }
        private ResetPasswordApiResponse ConvertToResetPasswordResponse(ApplicationUser user)
        {
            return new ResetPasswordApiResponse
            {

                Email = user.Email,
            };
            
        }
        private async ValueTask<ForgotPasswordApiResponse> ConvertToForgotPasswordResponse(ApplicationUser user)
        {

            await emailService.PostForgetPasswordMailRequestAsync(user, "Forgot Password Verification Token - Jaunts");

            return new  ForgotPasswordApiResponse
            {
                Email = user.Email
            };
            
        }
        private async ValueTask<UserProfileDetailsApiResponse> ConvertToConfirmEmailResponse(ApplicationUser user)
        {


            var role = await userManager.GetRolesAsync(user);

            //
            var userRoles = await roleManager.SelectAllRoles().Where(r => role.Contains(r.Name!)).ToListAsync();

            //
            var userPermissions = Permissions.None;
            //
            foreach (var rolePermission in userRoles)
                userPermissions |= rolePermission.Permissions;

            //
            var permissionsValue = (int)userPermissions;

            return new UserProfileDetailsApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = user.GenerateJwtToken(permissionsValue),
                Role = (List<string>)role
            };
            
        }

       
    }
}
