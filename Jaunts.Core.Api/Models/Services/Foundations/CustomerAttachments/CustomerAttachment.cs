using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments
{
    public class CustomerAttachment 
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
