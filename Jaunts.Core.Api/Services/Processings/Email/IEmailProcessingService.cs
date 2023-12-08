using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial interface IEmailProcessingService
    {
        ValueTask<SendEmailResponse> SendVerificationMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> SendForgetPasswordMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> SendOtpVerificationMailRequestAsync(
            ApplicationUser user,
            string token);
    }
}
