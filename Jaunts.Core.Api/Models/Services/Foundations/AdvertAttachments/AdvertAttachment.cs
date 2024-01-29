using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments
{
    public class AdvertAttachment 
    {
        public Guid AdvertId { get; set; }
        public Advert Advert { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
