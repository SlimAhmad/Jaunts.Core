using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments
{
    public class Attachment : IAuditable
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public byte[] Contents { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string ExternalUrl { get; set; }
        public AttachmentStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<CustomerAttachment> CustomerAttachments { get; set; }

        [JsonIgnore]
        public IEnumerable<ProviderAttachment> ProviderAttachments { get; set; }

        [JsonIgnore]
        public IEnumerable<ProvidersDirectorAttachment> ProvidersDirectorsAttachments { get; set; }
        [JsonIgnore]
        public IEnumerable<VacationPackagesAttachment> VacationPackagesAttachments { get; set; }
    }
}
