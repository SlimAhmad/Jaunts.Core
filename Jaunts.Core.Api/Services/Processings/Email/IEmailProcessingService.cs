using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial interface IEmailProcessingService
    {
        ValueTask<SendEmailResponse> VerificationMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> ForgetPasswordMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> OtpVerificationMailRequestAsync(
            ApplicationUser user,
            string token);
    }
}
