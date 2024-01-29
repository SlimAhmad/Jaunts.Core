// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeService
    {
        private void ValidateTransactionFeeOnRegister(TransactionFee transactionFee)
        {
            ValidateTransactionFee(transactionFee);

            Validate(
                (Rule: IsInvalid(transactionFee.Id), Parameter: nameof(TransactionFee.Id)),
                (Rule: IsInvalid(transactionFee.Name), Parameter: nameof(TransactionFee.Name)),
                (Rule: IsInvalid(transactionFee.Status), Parameter: nameof(TransactionFee.Status)),
                (Rule: IsInvalid(transactionFee.Description), Parameter: nameof(TransactionFee.Description)),
                (Rule: IsInvalid(transactionFee.CreatedBy), Parameter: nameof(TransactionFee.CreatedBy)),
                (Rule: IsInvalid(transactionFee.UpdatedBy), Parameter: nameof(TransactionFee.UpdatedBy)),
                (Rule: IsInvalid(transactionFee.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),
                (Rule: IsInvalid(transactionFee.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),
                (Rule: IsNotRecent(transactionFee.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),

                (Rule: IsNotSame(firstId: transactionFee.UpdatedBy,
                    secondId: transactionFee.CreatedBy,
                    secondIdName: nameof(TransactionFee.CreatedBy)),
                    Parameter: nameof(TransactionFee.UpdatedBy)),

                (Rule: IsNotSame(firstDate: transactionFee.UpdatedDate,
                    secondDate: transactionFee.CreatedDate,
                    secondDateName: nameof(TransactionFee.CreatedDate)),
                    Parameter: nameof(TransactionFee.UpdatedDate))
            );
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static dynamic IsInvalid(TransactionFeesStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateTransactionFeeId(Guid transactionFeeId)
        {
            Validate((Rule: IsInvalid(transactionFeeId), Parameter: nameof(TransactionFee.Id)));
        }

        private static void ValidateStorageTransactionFee(TransactionFee storageTransactionFee, Guid transactionFeeId)
        {
            if (storageTransactionFee is null)
            {
                throw new NotFoundTransactionFeeException(transactionFeeId);
            }
        }

        private void ValidateTransactionFeeOnModify(TransactionFee transactionFee)
        {
            ValidateTransactionFee(transactionFee);

            Validate(
                (Rule: IsInvalid(transactionFee.Id), Parameter: nameof(TransactionFee.Id)),
                (Rule: IsInvalid(transactionFee.Name), Parameter: nameof(TransactionFee.Name)),
                (Rule: IsInvalid(transactionFee.Status), Parameter: nameof(TransactionFee.Status)),
                (Rule: IsInvalid(transactionFee.Description), Parameter: nameof(TransactionFee.Description)),
                (Rule: IsInvalid(transactionFee.CreatedBy), Parameter: nameof(TransactionFee.CreatedBy)),
                (Rule: IsInvalid(transactionFee.UpdatedBy), Parameter: nameof(TransactionFee.UpdatedBy)),
                (Rule: IsInvalid(transactionFee.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),
                (Rule: IsInvalid(transactionFee.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),
                (Rule: IsNotRecent(transactionFee.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: transactionFee.UpdatedDate,
                    secondDate: transactionFee.CreatedDate,
                    secondDateName: nameof(TransactionFee.CreatedDate)),
                    Parameter: nameof(TransactionFee.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageTransactionFeeOnModify(TransactionFee inputTransactionFee, TransactionFee storageTransactionFee)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTransactionFee.CreatedDate,
                    secondDate: storageTransactionFee.CreatedDate,
                    secondDateName: nameof(TransactionFee.CreatedDate)),
                    Parameter: nameof(TransactionFee.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputTransactionFee.UpdatedDate,
                    secondDate: storageTransactionFee.UpdatedDate,
                    secondDateName: nameof(TransactionFee.UpdatedDate)),
                    Parameter: nameof(TransactionFee.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputTransactionFee.CreatedBy,
                    secondId: storageTransactionFee.CreatedBy,
                    secondIdName: nameof(TransactionFee.CreatedBy)),
                    Parameter: nameof(TransactionFee.CreatedBy))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateTransactionFee(TransactionFee transactionFee)
        {
            if (transactionFee is null)
            {
                throw new NullTransactionFeeException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTransactionFeeException = new InvalidTransactionFeeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionFeeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionFeeException.ThrowIfContainsErrors();
        }
    }
}