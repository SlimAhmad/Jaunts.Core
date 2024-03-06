using Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;

namespace Jaunts.Core.Api.Services.Processings.TransactionFees
{
    public partial class TransactionFeeProcessingService
    {
        private static void ValidateTransactionFee(TransactionFee fee)
        {
            ValidateTransactionFeeIsNotNull(fee);

            Validate(
            (Rule: IsInvalid(fee.Id),
                Parameter: nameof(TransactionFee.Id)));
        }
        private static void ValidateTransactionFeeIsNotNull(TransactionFee fee)
        {
            if (fee is null)
            {
                throw new NullTransactionFeeProcessingException();
            }
        }

        public void ValidateTransactionFeeList(IList<string> fees) =>
           Validate((Rule: IsInvalid(fees), Parameter: nameof(TransactionFee)));
        public void ValidateTransactionFeeId(Guid feeId) =>
           Validate((Rule: IsInvalid(feeId), Parameter: nameof(TransactionFee.Id)));
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
            var invalidTransactionFeeProcessingException = new InvalidTransactionFeeProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionFeeProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionFeeProcessingException.ThrowIfContainsErrors();
        }
    }
}