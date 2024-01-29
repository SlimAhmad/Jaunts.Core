using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments
{
    public class PromosOfferAttachment 
    {
        public Guid PromosOfferId { get; set; }
        public PromosOffer PromosOffer { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
