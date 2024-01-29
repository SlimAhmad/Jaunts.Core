using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDeals
{
    public class FlightDeal : IAuditable
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Airline { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public FlightDealsStatus Status { get; set; }
       

        public Guid ProviderId { get; set; }
        public Provider Providers { get; set; }
        public FlightDealAttachment FlightDealAttachment { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
