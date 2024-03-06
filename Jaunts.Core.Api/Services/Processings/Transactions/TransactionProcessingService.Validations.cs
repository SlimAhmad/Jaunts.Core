using Jaunts.Core.Api.Models.Processings.Transaction.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;

namespace Jaunts.Core.Api.Services.Processings.Transactions
{
    public partial class TransactionProcessingService
    {
        private static void ValidateTransaction(Transaction transaction)
        {
            ValidateTransactionIsNotNull(transaction);

            Validate(
            (Rule: IsInvalid(transaction.Id),
                Parameter: nameof(Transaction.Id)));
        }
        private static void ValidateTransactionIsNotNull(Transaction  transaction)
        {
            if (transaction is null)
            {
                throw new NullTransactionProcessingException();
            }
        }

        public void ValidateTransactionList(IList<string> transactions) =>
           Validate((Rule: IsInvalid(transactions), Parameter: nameof(Transaction)));
        public void ValidateTransactionId(Guid transactionId) =>
           Validate((Rule: IsInvalid(transactionId), Parameter: nameof(Transaction.Id)));
        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTransactionProcessingException = new InvalidTransactionProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionProcessingException.ThrowIfContainsErrors();
        }
    }
}