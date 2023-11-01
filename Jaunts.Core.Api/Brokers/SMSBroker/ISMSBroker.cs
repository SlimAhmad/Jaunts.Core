using Termii.Core.Models.Services.Foundations.Termii.Switch;
using Termii.Core.Models.Services.Foundations.Termii.Tokens;

namespace Jaunts.Core.Api.Brokers.SMSBroker
{
    public interface ISMSBroker
    {
        ValueTask<SendToken> PostTokenAsync(SendToken sendTokenRequest);
        ValueTask<SendMessage> PostMessage(SendMessage sendMessageRequest);
        ValueTask<VerifyToken> PostVerifyToken(VerifyToken verifyToken);
        ValueTask<EmailToken> PostEmailToken(EmailToken emailToken);
    }
}
