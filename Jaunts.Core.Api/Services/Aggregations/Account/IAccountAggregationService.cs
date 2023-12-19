using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial interface IAccountAggregationService
    {
        ValueTask<UserAccountDetailsResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerApiRequest);

        ValueTask<UserAccountDetailsResponse> LogInRequestAsync(
             LoginRequest loginApiRequest);

        ValueTask<bool> ResetPasswordRequestAsync(
           ResetPasswordApiRequest resetPassword);

        ValueTask<bool> ForgotPasswordRequestAsync(
             string email);

        ValueTask<UserAccountDetailsResponse> ConfirmEmailRequestAsync(
            string token,
            string email);

        ValueTask<UserAccountDetailsResponse> LoginWithOTPRequestAsync(
            string code,
            string userNameOrEmail);

        ValueTask<UserAccountDetailsResponse> EnableUser2FARequestAsync(Guid id);
    }
}
