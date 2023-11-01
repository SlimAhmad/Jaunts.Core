using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Brokers.EmailBroker
{
    public partial interface IEmailBroker
    {
        ValueTask<SendEmailResponse> PostMailAsync(SendEmailDetails sendEmailDetails);

      
    }
}
