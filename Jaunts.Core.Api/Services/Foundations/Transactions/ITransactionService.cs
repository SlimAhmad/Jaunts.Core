// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Transactions;

namespace Jaunts.Core.Api.Services.Foundations.Transactions
{
    public interface ITransactionService 
    {
        ValueTask<Transaction> CreateTransactionAsync(Transaction transaction);
        IQueryable<Transaction> RetrieveAllTransactions();
        ValueTask<Transaction> RetrieveTransactionByIdAsync(Guid transactionId);
        ValueTask<Transaction> ModifyTransactionAsync(Transaction transaction);
        ValueTask<Transaction> RemoveTransactionByIdAsync(Guid transactionId);
    }
}