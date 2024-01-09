using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments
{
    public class VacationPackagesAttachment 
    {
        public Guid VacationPackagesId { get; set; }
        public VacationPackage VacationPackage { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
