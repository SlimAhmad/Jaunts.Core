using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets
{
    public class Fleet : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public int SeatingCapacity { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string PlateNumber { get; set; }
        public FleetStatus Status { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Providers { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

      

    }
}
