using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments
{
    public class ProvidersDirectorAttachment 
    {
        public Guid ProviderDirectorId { get; set; }
        public ProvidersDirector ProviderDirector { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
