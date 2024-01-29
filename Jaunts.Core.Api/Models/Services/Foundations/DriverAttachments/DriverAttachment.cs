using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments
{
    public class DriverAttachment 
    {
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
