using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial interface IEmailService
    {
        ValueTask<SendEmailResponse> PostMailRequestAsync(SendEmailDetails email);

    }
}
