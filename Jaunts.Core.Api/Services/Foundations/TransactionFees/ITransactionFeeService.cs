// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;

namespace Jaunts.Core.Api.Services.Foundations.TransactionFees
{
    public interface ITransactionFeeService 
    {
        ValueTask<TransactionFee> CreateTransactionFeeAsync(TransactionFee shortLet);
        IQueryable<TransactionFee> RetrieveAllTransactionFees();
        ValueTask<TransactionFee> RetrieveTransactionFeeByIdAsync(Guid shortLetId);
        ValueTask<TransactionFee> ModifyTransactionFeeAsync(TransactionFee shortLet);
        ValueTask<TransactionFee> RemoveTransactionFeeByIdAsync(Guid shortLetId);
    }
}