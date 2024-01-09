using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<TransactionFee> InsertTransactionFeeAsync(
            TransactionFee transactionFee);

        IQueryable<TransactionFee> SelectAllTransactionFees();

        ValueTask<TransactionFee> SelectTransactionFeeByIdAsync(
            Guid transactionFeeId);

        ValueTask<TransactionFee> UpdateTransactionFeeAsync(
            TransactionFee transactionFee);

        ValueTask<TransactionFee> DeleteTransactionFeeAsync(
            TransactionFee transactionFee);
    }
}
