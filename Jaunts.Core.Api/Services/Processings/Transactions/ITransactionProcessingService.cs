using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Transactions
{
    public partial interface ITransactionProcessingService
    {
        IQueryable<Transaction> RetrieveAllTransactions();
        ValueTask<bool> RemoveTransactionAsync(Guid id);
        ValueTask<Transaction> RetrieveTransactionByIdAsync(Guid id);
        ValueTask<Transaction> UpsertTransactionAsync(Transaction Transaction);
        ValueTask<bool> EnsureTransactionExistAsync(Transaction transaction);
        ValueTask<bool> VerifyTransactionsExistAsync();

    }
}
