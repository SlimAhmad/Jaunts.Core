using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers
{
    public class PromosOffer : IAuditable
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string CodeOrName { get; set; }
        public PromoType Type { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal DiscountPercentage { get; set; }
        public Decimal MinimumPurchaseAmount { get; set; }
        public int MaximumUsageLimit { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public PromosOffersStatus Status { get; set; }

        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public Service Service { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public PromosOfferAttachment PromosOfferAttachment { get; set; }

    }
}
