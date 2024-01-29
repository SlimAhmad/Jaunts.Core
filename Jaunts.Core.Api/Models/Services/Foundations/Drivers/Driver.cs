using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers
{
    public class Driver : IAuditable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string ContactNumber { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public Guid FleetId { get; set; }
        public Fleet Fleet { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<DriverAttachment> DriverAttachments { get; set; }
    }
}
  