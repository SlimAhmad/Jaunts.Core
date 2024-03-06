using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using System.Text.Json.Serialization;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances
{
    public class WalletBalance : IAuditable
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public decimal Balance { get; set; }
        public bool IsArchived { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        
    }
}
