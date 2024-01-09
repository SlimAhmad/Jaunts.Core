using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers
{
    public class Provider : IAuditable
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string RcNumber { get; set; }
        public string Address { get; set; }
        public DateTimeOffset Incorporation { get; set; }
        public ProviderStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<ProvidersDirector> ProvidersDirectors { get; set; }

        [JsonIgnore]
        public IEnumerable<ProviderService> ProviderServices { get; set; }
    }
}
