using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory
{
    public class ProviderCategory : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<Provider> Providers { get; set; }
    }
}
