using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial interface IEmailService
    {
  
        ValueTask<SendEmailResponse> PostVerificationMailRequestAsync(ApplicationUser user, 
            string subject,string token,string from,string fromName);
        ValueTask<SendEmailResponse> PostForgetPasswordMailRequestAsync(
            ApplicationUser user,
            string subject,
            string token,
            string from,
            string fromName);
        ValueTask<SendEmailResponse> PostOTPVerificationMailRequestAsync(
            ApplicationUser user,
            string subject,
            string token,
            string from,
            string fromName);

     




    }
}
