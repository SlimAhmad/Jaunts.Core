using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments
{
    public class RideAttachment
    {
        public Guid RideId { get; set; }
        public Ride Ride { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
