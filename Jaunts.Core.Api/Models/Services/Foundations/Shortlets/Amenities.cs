using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets
{
    public class Amenities 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ShortLetId { get; set; }
        public ShortLet ShortLet { get; set; }

    }
}
