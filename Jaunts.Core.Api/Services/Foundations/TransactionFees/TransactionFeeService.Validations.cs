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
        private void ValidateTransactionFeeOnRegister(TransactionFee shortLet)
        {
            ValidateTransactionFee(shortLet);

            Validate(
                (Rule: IsInvalid(shortLet.Id), Parameter: nameof(TransactionFee.Id)),
                (Rule: IsInvalid(shortLet.Name), Parameter: nameof(TransactionFee.Name)),
                (Rule: IsInvalid(shortLet.Description), Parameter: nameof(TransactionFee.Description)),
                (Rule: IsInvalid(shortLet.CreatedBy), Parameter: nameof(TransactionFee.CreatedBy)),
                (Rule: IsInvalid(shortLet.UpdatedBy), Parameter: nameof(TransactionFee.UpdatedBy)),
                (Rule: IsInvalid(shortLet.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),
                (Rule: IsInvalid(shortLet.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),
                (Rule: IsNotRecent(shortLet.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),

                (Rule: IsNotSame(firstId: shortLet.UpdatedBy,
                    secondId: shortLet.CreatedBy,
                    secondIdName: nameof(TransactionFee.CreatedBy)),
                    Parameter: nameof(TransactionFee.UpdatedBy)),

                (Rule: IsNotSame(firstDate: shortLet.UpdatedDate,
                    secondDate: shortLet.CreatedDate,
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

        private static void ValidateTransactionFeeId(Guid shortLetId)
        {
            if (shortLetId == Guid.Empty)
            {
                throw new InvalidTransactionFeeException(
                    parameterName: nameof(TransactionFee.Id),
                    parameterValue: shortLetId);
            }
        }

        private static void ValidateStorageTransactionFee(TransactionFee storageTransactionFee, Guid shortLetId)
        {
            if (storageTransactionFee is null)
            {
                throw new NotFoundTransactionFeeException(shortLetId);
            }
        }

        private void ValidateTransactionFeeOnModify(TransactionFee shortLet)
        {
            ValidateTransactionFee(shortLet);

            Validate(
                (Rule: IsInvalid(shortLet.Id), Parameter: nameof(TransactionFee.Id)),
                (Rule: IsInvalid(shortLet.Name), Parameter: nameof(TransactionFee.Name)),
                (Rule: IsInvalid(shortLet.Description), Parameter: nameof(TransactionFee.Description)),
                (Rule: IsInvalid(shortLet.CreatedBy), Parameter: nameof(TransactionFee.CreatedBy)),
                (Rule: IsInvalid(shortLet.UpdatedBy), Parameter: nameof(TransactionFee.UpdatedBy)),
                (Rule: IsInvalid(shortLet.CreatedDate), Parameter: nameof(TransactionFee.CreatedDate)),
                (Rule: IsInvalid(shortLet.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),
                (Rule: IsNotRecent(shortLet.UpdatedDate), Parameter: nameof(TransactionFee.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: shortLet.UpdatedDate,
                    secondDate: shortLet.CreatedDate,
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

        private static void ValidateTransactionFee(TransactionFee shortLet)
        {
            if (shortLet is null)
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