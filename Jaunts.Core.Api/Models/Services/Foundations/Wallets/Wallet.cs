using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets
{
    public class Wallet : IAuditable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string WalletName { get; set; }
        public string Description { get; set; }
        public WalletStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<WalletBalance> WalletBalances { get; set; }
    }
}
