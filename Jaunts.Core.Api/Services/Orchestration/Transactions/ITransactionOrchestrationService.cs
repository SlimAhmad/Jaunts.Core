using Jaunts.Core.Api.Models.Services.Foundations.Transactions;

namespace Jaunts.Core.Api.Services.Orchestration.Transactions
{
    public partial interface ITransactionOrchestrationService
    {
        IQueryable<Transaction> RetrieveAllTransactionsAsync();
        ValueTask<bool> RemoveTransactionByIdAsync(Guid id);
        ValueTask<Transaction> RetrieveTransactionAsync(Guid id);
        ValueTask<Transaction> UpsertTransactionAsync(Transaction Transaction);
    }
}
