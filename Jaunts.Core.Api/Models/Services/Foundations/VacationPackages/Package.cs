using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages
{
    public class Package : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public PackageStatus Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<PackageAttachment> PackageAttachments { get; set; }
    }
}
