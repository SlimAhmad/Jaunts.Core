using Termii.Core.Models.Services.Foundations.Termii.Switch;
using Termii.Core.Models.Services.Foundations.Termii.Tokens;

namespace Jaunts.Core.Api.Services.Foundations.SMS
{
    public partial interface ISMSService
    {
        ValueTask<SendToken> PostTokenRequestAsync(SendToken sendTokenRequest);
        ValueTask<SendMessage> PostMessageRequestAsync(SendMessage sendMessageRequest);
        ValueTask<VerifyToken> PostVerifyTokenRequestAsync(VerifyToken verifyToken);
        ValueTask<EmailToken> PostEmailTokenRequestAsync(EmailToken emailToken);
    }
}
