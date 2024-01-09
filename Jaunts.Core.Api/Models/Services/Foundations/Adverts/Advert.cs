using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts
{
    public class Advert : IAuditable
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public AdvertsStatus Status { get; set; }
        public AdvertsPlacement Placement { get; set; }
        public Guid TransactionFeeId { get; set; }
        public TransactionFee TransactionFee { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
