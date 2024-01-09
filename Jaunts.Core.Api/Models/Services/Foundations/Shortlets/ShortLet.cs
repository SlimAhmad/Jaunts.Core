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
        public ShortLetsStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<Amenities> Amenities { get; set; }
    }
}
