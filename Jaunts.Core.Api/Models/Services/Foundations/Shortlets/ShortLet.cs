using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets
{
    public class ShortLet: IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public Decimal PricePerNight { get; set; }
        public ShortLetStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<Amenity> Amenities { get; set; }

        [JsonIgnore]
        public IEnumerable<ShortLetAttachment> ShortLetAttachments { get; set; }
    }
}
