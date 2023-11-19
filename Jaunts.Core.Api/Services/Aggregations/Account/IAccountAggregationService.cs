using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial interface IAccountAggregationService
    {
        ValueTask<UserAccountDetailsApiResponse> RegisterUserRequestAsync(
            RegisterUserApiRequest registerCredentialsApiRequest);

        ValueTask<UserAccountDetailsApiResponse> LogInRequestAsync(
             LoginCredentialsApiRequest loginCredentialsApiRequest);

        ValueTask<bool> ResetPasswordRequestAsync(
           ResetPasswordApiRequest resetPassword);

        ValueTask<bool> ForgotPasswordRequestAsync(
             string email);

        ValueTask<UserAccountDetailsApiResponse> ConfirmEmailRequestAsync(
            string token,
            string email);

        ValueTask<UserAccountDetailsApiResponse> LoginWithOTPRequestAsync(
            string code,
            string userNameOrEmail);

        ValueTask<UserAccountDetailsApiResponse> EnableUser2FARequestAsync(Guid id);
    }
}
