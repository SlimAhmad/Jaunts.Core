using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments
{
    public class ProviderAttachment 
    {
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
