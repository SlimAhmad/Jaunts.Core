using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial interface IEmailProcessingService
    {
        ValueTask<SendEmailResponse> PostVerificationMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> PostForgetPasswordMailRequestAsync(
            ApplicationUser user,
            string token);
        ValueTask<SendEmailResponse> PostOTPVerificationMailRequestAsync(
            ApplicationUser user,
            string token);
    }
}
