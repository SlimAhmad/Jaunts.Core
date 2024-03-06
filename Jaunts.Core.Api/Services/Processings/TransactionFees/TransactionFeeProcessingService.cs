using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Services.Foundations.TransactionFees;
using System.Linq.Expressions;

namespace Jaunts.Core.Api.Services.Processings.TransactionFees
{
    public partial class TransactionFeeProcessingService : ITransactionFeeProcessingService
    {
      
        private readonly ITransactionFeeService  transactionFeeService;
        private readonly ILoggingBroker loggingBroker;

        public TransactionFeeProcessingService(
            ITransactionFeeService  TransactionFeeService,
            ILoggingBroker loggingBroker

            )
        {
            this.transactionFeeService = TransactionFeeService;
            this.loggingBroker = loggingBroker;

        }

        public IQueryable<TransactionFee> RetrieveAllTransactionFees() =>
        TryCatch(() => this.transactionFeeService.RetrieveAllTransactionFees());
        public ValueTask<bool> RemoveTransactionFeeByIdAsync(
            Guid id) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeId(id);
            var transactionFee = await transactionFeeService.RemoveTransactionFeeByIdAsync(id);
            ValidateTransactionFee(transactionFee);
            return true;
        });

        public ValueTask<TransactionFee> RetrieveTransactionFeeByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateTransactionFeeId(id);
            var wallet = await transactionFeeService.RetrieveTransactionFeeByIdAsync(id);
            ValidateTransactionFee(wallet);
            return wallet;
        });

        public ValueTask<TransactionFee> UpsertTransactionFeeAsync(
                TransactionFee  transactionFee) =>
            TryCatch(async () =>
            {
                ValidateTransactionFee(transactionFee);
                TransactionFee maybeTransactionFee = RetrieveMatchingTransactionFee(transactionFee);

                return maybeTransactionFee switch
                {
                    null => await this.transactionFeeService.CreateTransactionFeeAsync(transactionFee),
                    _ => await this.transactionFeeService.ModifyTransactionFeeAsync(transactionFee)
                };
            });

        private TransactionFee RetrieveMatchingTransactionFee(TransactionFee transactionFee)
        {
            IQueryable<TransactionFee> TransactionFees =
                this.transactionFeeService.RetrieveAllTransactionFees();

            return TransactionFees.FirstOrDefault(SameTransactionFeeAs(transactionFee));
        }

        private static Expression<Func<TransactionFee, bool>> SameTransactionFeeAs(TransactionFee transactionFee) =>
            retrieveTransactionFee => retrieveTransactionFee.Id == transactionFee.Id;





    }
}
