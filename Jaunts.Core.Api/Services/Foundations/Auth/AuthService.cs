using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Authorization;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Web;

namespace Jaunts.Core.Api.Services.Foundations.Auth
{
    public partial class AuthService : IAuthService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly IRoleManagementBroker roleManagementBroker;
        private readonly ISignInManagementBroker signInManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public AuthService(
            IUserManagementBroker userManagementBroker,
            ISignInManagementBroker signInManagementBroker,
            IRoleManagementBroker roleManager,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.roleManagementBroker = roleManager;
            this.signInManagementBroker = signInManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }



        public ValueTask<RegisterResultApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnRegister(registerCredentialsApiRequest);
            ApplicationUser  registerUserRequest = ConvertToAuthRequest(registerCredentialsApiRequest);
            IdentityResult registerUserResponse = 
                await userManagementBroker.RegisterUserAsync(registerUserRequest, registerCredentialsApiRequest.Password);
            ValidateIdentityResultResponse(registerUserResponse);
            ValidateUserResponse(registerUserRequest);
            return await ConvertToRegisterResponse(registerUserRequest);
        });
        public ValueTask<UserProfileDetailsApiResponse> LogInRequestAsync(
            LoginCredentialsApiRequest loginCredentialsApiRequest) =>
        TryCatch(async () =>
        {
        
            ValidateUserOnLogin(loginCredentialsApiRequest);
            var isEmail = loginCredentialsApiRequest.UsernameOrEmail.Contains("@");
            var user = isEmail ?
                await userManagementBroker.FindByEmailAsync(loginCredentialsApiRequest.UsernameOrEmail) :
                await userManagementBroker.FindByNameAsync(loginCredentialsApiRequest.UsernameOrEmail);

            if (user.TwoFactorEnabled)
            {
                await signInManagementBroker.SignOutAsync();
                await signInManagementBroker.PasswordSignInAsync(user, loginCredentialsApiRequest.Password,false,true);
                var token =
                     await userManagementBroker.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
                var response = await emailService.PostOTPVerificationMailRequestAsync(
                    user, "2FA Token - Jaunts", 
                    token,
                    configuration.GetSection("MailTrap:From").ToString(),
                    configuration.GetSection("MailTrap:FromName").ToString());
                if(response.Successful)
                    return  ConvertTo2FAResponse(user);
            }
      
            ValidateUserResponse(user);
            var isValidPassword =
                        await userManagementBroker.CheckPasswordAsync(user, loginCredentialsApiRequest.Password);
            ValidateUserPassword(isValidPassword);
            return await ConvertToLoginResponse(user);
        });
        public  ValueTask<ResetPasswordApiResponse> ResetPasswordRequestAsync(ResetPasswordApiRequest resetPassword) =>
        TryCatch(async () =>
        {
            ValidateResetPassword(resetPassword);
            var user = await userManagementBroker.FindByEmailAsync(resetPassword.Email);
            ValidateUserResponse(user);
            var response = await userManagementBroker.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetPassword.Token), resetPassword.Password);
            ValidateIdentityResultResponse(response);
            return ConvertToResetPasswordResponse(user);
        });
        public ValueTask<ForgotPasswordApiResponse> ForgotPasswordRequestAsync(string email) =>
        TryCatch(async () =>
        {
            ValidateUserEmail(email);
            var user = await userManagementBroker.FindByEmailAsync(email);
            ValidateUserResponse(user);
            return await ConvertToForgotPasswordResponse(user);

        });
        public ValueTask<UserProfileDetailsApiResponse> ConfirmEmailRequestAsync(string token, string email) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(token);
            ValidateUserProfileDetails(email);
            var user = await userManagementBroker.FindByEmailAsync(email);
            ValidateUserResponse(user);
            var identityResult = await userManagementBroker.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            ValidateIdentityResultResponse(identityResult);
            return await ConvertToConfirmEmailResponse(user);
        });
        public ValueTask<UserProfileDetailsApiResponse> LoginWithOTPRequestAsync(string code, string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(userNameOrEmail);
            ValidateUserProfileDetails(code);
            await signInManagementBroker.TwoFactorSignInAsync(TokenOptions.DefaultPhoneProvider, code, false, false);

            var isEmail = userNameOrEmail.Contains("@");
            var user = isEmail ?
               await userManagementBroker.FindByEmailAsync(userNameOrEmail) :
               await userManagementBroker.FindByNameAsync(userNameOrEmail);
            ValidateUserResponse(user);
            return await ConvertToLoginResponse(user);
        });
        public ValueTask<Enable2FAApiResponse> EnableUser2FARequestAsync(Guid id) =>
        TryCatch(async () =>
        {
           
            var user = await userManagementBroker.FindByIdAsync(id.ToString());
            ValidateUserResponse(user);         
            var enable = user.TwoFactorEnabled ?
               await userManagementBroker.SetTwoFactorEnabledAsync(user,false) :
               await userManagementBroker.SetTwoFactorEnabledAsync(user,true);
            ValidateIdentityResultResponse(enable);
            await userManagementBroker.UpdateUserAsync(user);

            return new Enable2FAApiResponse();
        });



        private async ValueTask<UserProfileDetailsApiResponse> ConvertToLoginResponse(ApplicationUser user)
        {

            var role = await userManagementBroker.GetRolesAsync(user);
            var userRoles = await roleManagementBroker.SelectAllRoles().Where(r => role.Contains(r.Name!)).ToListAsync();

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
                Role = (List<string>)role,
                TwoFactorEnabled = user.TwoFactorEnabled,
              
            };
            
        }
        private  UserProfileDetailsApiResponse ConvertTo2FAResponse(ApplicationUser user)
        {

            return new UserProfileDetailsApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TwoFactorEnabled = user.TwoFactorEnabled,
            };

        }
        private async ValueTask<RegisterResultApiResponse> ConvertToRegisterResponse(ApplicationUser user)
        {

            await userManagementBroker.AddToRoleAsync(user, "User");

            var role = await userManagementBroker.GetRolesAsync(user);

            //
            var userRoles = await roleManagementBroker.SelectAllRoles().Where(r => role.Contains(r.Name!)).ToListAsync();

            //
            var userPermissions = Permissions.None;
            //
            foreach (var rolePermission in userRoles)
                userPermissions |= rolePermission.Permissions;

            //
            var permissionsValue = (int)userPermissions;

            var token = await userManagementBroker.GenerateEmailConfirmationTokenAsync(user);
            await emailService.PostVerificationMailRequestAsync(
                user, 
                "Verify Your Email - Jaunts",
                token,
                configuration.GetSection("MailTrap:From").ToString(),
                configuration.GetSection("MailTrap:FromName").ToString());

            return new RegisterResultApiResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id.ToString(),
                Token = user.GenerateJwtToken(permissionsValue),
                Role = (List<string>)role,
                TwoFactorEnabled = user.TwoFactorEnabled,
               
            };
            
        }
        private ApplicationUser ConvertToAuthRequest(RegisterUserApiRequest registerUserApiRequest)
        {
            return new ApplicationUser
            {
                UserName = registerUserApiRequest.Username,
                FirstName = registerUserApiRequest.FirstName,
                LastName = registerUserApiRequest.LastName,
                Email = registerUserApiRequest.Email,
                PhoneNumber = registerUserApiRequest.PhoneNumber
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
            var token = await userManagementBroker.GeneratePasswordResetTokenAsync(user);
            await emailService.PostForgetPasswordMailRequestAsync(user, "Forgot Password Verification Token - Jaunts",token, "ui", "uu");

            return new  ForgotPasswordApiResponse
            {
                Email = user.Email
            };
            
        }
        private async ValueTask<UserProfileDetailsApiResponse> ConvertToConfirmEmailResponse(ApplicationUser user)
        {

            var role = await userManagementBroker.GetRolesAsync(user);

            //
            var userRole =  roleManagementBroker.SelectAllRoles();
            var userRoles =  userRole.Where(r => role.Contains(r.Name!)).ToList();
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
