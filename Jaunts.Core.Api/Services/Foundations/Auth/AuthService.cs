using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleManagementBroker roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AuthService(
            IUserManagementBroker userManagementBroker,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IRoleManagementBroker roleManager,
            IEmailService emailService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }



        public ValueTask<RegisterResultApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest) =>
        TryCatch(async () =>
        {
            ValidateUserOnRegister(registerCredentialsApiRequest);
            ApplicationUser  registerUserRequest = ConvertToAuthRequest(registerCredentialsApiRequest);
            ApplicationUser registerUserResponse = 
                await userManagementBroker.InsertUserAsync(registerUserRequest, registerCredentialsApiRequest.Password);
            ValidateUserResponse(registerUserResponse);
            return await ConvertToRegisterResponse(registerUserRequest);
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

            if (user.TwoFactorEnabled)
            {
                await signInManager.SignOutAsync();
                await signInManager.PasswordSignInAsync(user, loginCredentialsApiRequest.Password,false,true);
                var token =
                     await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
                var response = await emailService.PostOTPVerificationMailRequestAsync(user, "2FA Token - Jaunts",token,"ui","uu");
                if(response.Successful)
                    return  ConvertTo2FAResponse(user);
            }
      
            ValidateUserResponse(user);
            var isValidPassword =
                        await userManager.CheckPasswordAsync(user, loginCredentialsApiRequest.Password);
            ValidateUserPassword(isValidPassword);
            return await ConvertToLoginResponse(user);
        });

      

        public  ValueTask<ResetPasswordApiResponse> ResetPasswordRequestAsync(ResetPasswordApiRequest resetPassword) =>
        TryCatch(async () =>
        {
            ValidateResetPassword(resetPassword);
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            ValidateUserResponse(user);
            var response = await userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetPassword.Token), resetPassword.Password);
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
            var identityResult = await userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            ValidateIdentityResultResponse(identityResult);
            return await ConvertToConfirmEmailResponse(user);
        });
        public ValueTask<UserProfileDetailsApiResponse> LoginWithOTPRequestAsync(string code, string userNameOrEmail) =>
        TryCatch(async () =>
        {
            ValidateUserProfileDetails(userNameOrEmail);
            ValidateUserProfileDetails(code);
            var signIn = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultPhoneProvider, code, false, false);
            ValidateSignIn(signIn.Succeeded);
            var isEmail = userNameOrEmail.Contains("@");
            var user = isEmail ?
               await userManager.FindByEmailAsync(userNameOrEmail) :
               await userManager.FindByNameAsync(userNameOrEmail);
            ValidateUserResponse(user);
            return await ConvertToLoginResponse(user);
        });
        public ValueTask<Enable2FAApiResponse> EnableUser2FARequestAsync(Guid id) =>
        TryCatch(async () =>
        {
           
            var user = await userManager.FindByIdAsync(id.ToString());
            ValidateUserResponse(user);         
            var enable = user.TwoFactorEnabled ?
               await userManager.SetTwoFactorEnabledAsync(user,false) :
               await userManager.SetTwoFactorEnabledAsync(user,true);
            ValidateIdentityResultResponse(enable);
            await userManager.UpdateAsync(user);

            return new Enable2FAApiResponse();
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

            var token = await userManagementBroker.GenerateEmailConfirmationTokenAsync(user);
            await emailService.PostVerificationMailRequestAsync(user, "Verify Your Email - Jaunts",token,"hj","hh");

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
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await emailService.PostForgetPasswordMailRequestAsync(user, "Forgot Password Verification Token - Jaunts",token, "ui", "uu");

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
