using Termii.Core.Clients.Termii;
using Termii.Core.Models.Configurations;
using Termii.Core.Models.Services.Foundations.Termii.Switch;
using Termii.Core.Models.Services.Foundations.Termii.Tokens;

namespace Jaunts.Core.Api.Brokers.SMSBroker
{
    public partial class SMSBroker : ISMSBroker
    {

        private readonly ITermiiClient termiiClient;

        public SMSBroker()
        {
            var apiConfigurations = new ApiConfigurations
            {

                ApiKey = Environment.GetEnvironmentVariable("ApiKey"),

            };

          
           this.termiiClient = new TermiiClient(apiConfigurations);
        }

        public async ValueTask<EmailToken> PostEmailToken(EmailToken emailToken)
        {
            return await this.termiiClient.Tokens.SendEmailTokenAsync(emailToken);
        }

        public async ValueTask<SendMessage> PostMessage(SendMessage sendMessageRequest)
        {
           return  await this.termiiClient.Switch.SendMessageAsync(sendMessageRequest);
        }

        public async ValueTask<SendToken> PostTokenAsync(SendToken sendTokenRequest)
        {
            return await this.termiiClient.Tokens.SendTokenAsync(sendTokenRequest);
        }

        public async ValueTask<VerifyToken> PostVerifyToken(VerifyToken verifyToken)
        {
            return await this.termiiClient.Tokens.SendVerifyTokenAsync(verifyToken);
        }
    }
}
