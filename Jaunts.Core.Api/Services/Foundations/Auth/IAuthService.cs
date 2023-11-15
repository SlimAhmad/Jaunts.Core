using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Foundations.Auth
{
    public interface IAuthService
    {
        ValueTask<RegisterResultApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest);

        ValueTask<UserProfileDetailsApiResponse> LogInRequestAsync(
             LoginCredentialsApiRequest loginCredentialsApiRequest);

        ValueTask<ResetPasswordApiResponse> ResetPasswordRequestAsync(
           ResetPasswordApiRequest resetPassword);

        ValueTask<ForgotPasswordApiResponse> ForgotPasswordRequestAsync(
             string email);

        ValueTask<UserProfileDetailsApiResponse> ConfirmEmailRequestAsync(
            string token,
            string email);

        ValueTask<UserProfileDetailsApiResponse> LoginWithOTPRequestAsync(
            string code,
            string userNameOrEmail);

        ValueTask<Enable2FAApiResponse> EnableUser2FARequestAsync(Guid id);
    }
}
