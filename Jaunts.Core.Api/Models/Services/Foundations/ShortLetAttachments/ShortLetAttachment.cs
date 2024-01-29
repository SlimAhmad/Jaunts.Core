using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments
{
    public class ShortLetAttachment
    {
        public Guid ShortLetId { get; set; }
        public ShortLet ShortLet { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
