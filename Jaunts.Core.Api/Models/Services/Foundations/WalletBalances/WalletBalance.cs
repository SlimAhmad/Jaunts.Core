namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances
{
    public class WalletBalance : IAuditable
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public bool IsArchived { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
