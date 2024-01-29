using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides
{
    public class Ride
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public Guid FleetId { get; set; }
        public Fleet Fleet { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public RideStatus RideStatus { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<RideAttachment> RideAttachments { get; set; }
    }
}
