using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Brokers.EmailBroker
{
    public partial class EmailBroker
    {

        public async ValueTask<SendEmailResponse> PostMailAsync(SendEmailDetails sendEmailDetails)
        {
            return await PostAsync<SendEmailDetails, SendEmailResponse>(
               relativeUrl: $"api/send/2460224",
              
               content: sendEmailDetails);
        }

       
    }
}
