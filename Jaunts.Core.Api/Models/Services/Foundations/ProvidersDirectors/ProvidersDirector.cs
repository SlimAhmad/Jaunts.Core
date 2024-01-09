using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors
{
    public class ProvidersDirector : IAuditable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Position { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
