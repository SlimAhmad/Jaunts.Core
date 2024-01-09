using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial interface IAccountAggregationService
    {
        ValueTask<UserAccountDetailsResponse> RegisterUserRequestAsync(
            UserCredentialsRequest registerApiRequest);

        ValueTask<UserAccountDetailsResponse> LogInRequestAsync(
             LoginRequest loginApiRequest);

        ValueTask<bool> ResetPasswordRequestAsync(
           ResetPasswordRequest resetPassword);

        ValueTask<bool> ForgotPasswordRequestAsync(
             string email);

        ValueTask<UserAccountDetailsResponse> EmailConfirmationAsync(
            string token,
            string email);

        ValueTask<UserAccountDetailsResponse> OtpLoginRequestAsync(
            string code,
            string userNameOrEmail);

        ValueTask<UserAccountDetailsResponse> EnableUserTwoFactorAsync(Guid id);
    }
}
