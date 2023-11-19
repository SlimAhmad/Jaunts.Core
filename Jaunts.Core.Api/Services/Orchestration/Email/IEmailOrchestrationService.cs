using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Orchestration.Email
{
    public partial interface IEmailOrchestrationService
    {
        ValueTask<SendEmailResponse> VerificationMailAsync(
                ApplicationUser user);
        ValueTask<SendEmailResponse> PasswordResetMailAsync(
                ApplicationUser user);
        ValueTask<SendEmailResponse> TwoFactorMailAsync(
                ApplicationUser user);
    }
}
