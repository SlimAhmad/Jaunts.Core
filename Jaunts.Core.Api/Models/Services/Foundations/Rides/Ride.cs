using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;

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
        public Guid CarId { get; set; }
        public Fleet Car { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public RidesStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
