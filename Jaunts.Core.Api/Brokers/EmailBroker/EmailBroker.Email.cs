using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Brokers.EmailBroker
{
    public partial class EmailBroker
    {

        public async ValueTask<SendEmailResponse> SendEmailAsync(SendEmailMessage sendEmailDetails)
        {
            return await PostAsync<SendEmailMessage, SendEmailResponse>(
               relativeUrl: $"api/send/2460224",
              
               content: sendEmailDetails);
        }

       
    }
}
