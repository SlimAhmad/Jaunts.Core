using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions
{
    public class Transaction : IAuditable
    {
        public Guid Id { get; set; }
        public Guid WalletBalanceId { get; set; }
        public WalletBalance WalletBalance { get; set; }
        public Guid TransactionFeeId { get; set; }
        public TransactionFee TransactionFee { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus Status { get; set; }    
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Narration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
