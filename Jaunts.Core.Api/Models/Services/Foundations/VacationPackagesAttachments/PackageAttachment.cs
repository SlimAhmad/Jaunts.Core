using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments
{
    public class PackageAttachment 
    {
        public Guid PackageId { get; set; }
        public Package Package { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
