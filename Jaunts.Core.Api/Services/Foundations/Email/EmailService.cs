using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Models.Email;

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
     
        public ValueTask<SendEmailResponse> SendEmailRequestAsync(SendEmailMessage sendEmailDetails) =>
        TryCatch(async () =>
        {
            ValidateMail(sendEmailDetails);
            SendEmailResponse emailResponse = await sendEmailDetailsBroker.SendEmailAsync(sendEmailDetails);
            ValidateMailResponse(emailResponse);
            return emailResponse;
        });


    }
}
