using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenities
{
    public class Amenity : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ShortLetId { get; set; }
        public ShortLet ShortLet { get; set; }
        public DateTimeOffset CreatedDate { get; set ; }
        public DateTimeOffset UpdatedDate { get ; set ; }
        public Guid CreatedBy { get ; set ; }
        public Guid UpdatedBy { get ; set ; }
    }
}
