using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial class EmailService : IEmailService
    {
        private readonly IEmailBroker sendEmailDetailsBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;


        public EmailService(
            IEmailBroker sendEmailDetailsBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker

            )
        {
            this.sendEmailDetailsBroker = sendEmailDetailsBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;   
        }

     
        public ValueTask<SendEmailResponse> PostMailRequestAsync(SendEmailDetails sendEmailDetails) =>
        TryCatch(async () => {
           ValidateMail(sendEmailDetails);
            SendEmailResponse emailResponse = await sendEmailDetailsBroker.PostMailAsync(sendEmailDetails);
            ValidateMailResponse(emailResponse);
            return emailResponse;
        });


    }
}
