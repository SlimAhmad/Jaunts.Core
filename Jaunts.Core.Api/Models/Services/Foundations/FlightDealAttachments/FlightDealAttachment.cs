using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments
{
    public class FlightDealAttachment 
    {
        public Guid FlightDealId { get; set; }
        public FlightDeal FlightDeal { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
