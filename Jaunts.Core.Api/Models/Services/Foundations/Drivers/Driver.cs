using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;

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
        public DriversStatus DriversStatus { get; set; }
        public Guid CarId { get; set; }
        public Guid VendorId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
  