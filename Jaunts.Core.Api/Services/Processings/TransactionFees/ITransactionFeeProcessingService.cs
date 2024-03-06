using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.TransactionFees
{
    public partial interface ITransactionFeeProcessingService
    {
        IQueryable<TransactionFee> RetrieveAllTransactionFees();
        ValueTask<bool> RemoveTransactionFeeByIdAsync(Guid id);
        ValueTask<TransactionFee> RetrieveTransactionFeeByIdAsync(Guid id);
        ValueTask<TransactionFee> UpsertTransactionFeeAsync(TransactionFee fee);

    }
}
