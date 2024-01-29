using Jaunts.Core.Api.Models.Services.Foundations.Transactions;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Transaction> InsertTransactionAsync(
            Transaction transaction);

        IQueryable<Transaction> SelectAllTransactions();

        ValueTask<Transaction> SelectTransactionByIdAsync(
            Guid transactionId);

        ValueTask<Transaction> UpdateTransactionAsync(
            Transaction transaction);

        ValueTask<Transaction> DeleteTransactionAsync(
            Transaction transaction);
    }
}
