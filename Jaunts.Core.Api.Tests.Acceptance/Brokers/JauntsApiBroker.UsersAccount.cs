using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Routes;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers
{
    public partial class JauntsApiBroker
    {

        public async ValueTask<UserAccountDetailsResponse> RegisterUserRequestAsync(UserCredentialsRequest userCredentials) =>
           await this.apiFactoryClient.PostContentAsync<UserCredentialsRequest,UserAccountDetailsResponse>(
                  ApiRoutes.Register, userCredentials);
        public async ValueTask<UserAccountDetailsResponse> LogInRequestAsync(LoginRequest loginApiRequest) =>
           await this.apiFactoryClient.PostContentAsync<LoginRequest, UserAccountDetailsResponse>(
                  ApiRoutes.Login, loginApiRequest);
        public async ValueTask<bool> ResetPasswordRequestAsync(
           ResetPasswordRequest resetPassword) =>
           await this.apiFactoryClient.PostContentAsync<ResetPasswordRequest, bool>(
                 ApiRoutes.ResetPassword, resetPassword);
        public async ValueTask<bool> ForgotPasswordRequestAsync(string email) =>
           await this.apiFactoryClient.PutContentAsync<bool>($"{ApiRoutes.ForgotPassword}?email={email}");
        public async ValueTask<UserAccountDetailsResponse> EmailConfirmationAsync(string token,string email) =>
            await this.apiFactoryClient.PutContentAsync<UserAccountDetailsResponse>($"{ApiRoutes.ConfirmEmail}/{email}/{token}");
        public async ValueTask<UserAccountDetailsResponse> OtpLoginRequestAsync(string code,string userNameOrEmail) =>
            await this.apiFactoryClient.PutContentAsync<UserAccountDetailsResponse>($"{ApiRoutes.OtpLogin}/{userNameOrEmail}/{code}");
        public async ValueTask<UserAccountDetailsResponse> EnableUserTwoFactorAsync(Guid userId) =>
            await this.apiFactoryClient.PutContentAsync<UserAccountDetailsResponse>($"{ApiRoutes.EnableTwoFactor}?id={userId}");

    }
}
